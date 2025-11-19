using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] float sensitivity;

    [Header("Movement Tilt")]
    [SerializeField] float tiltAngle;
    [SerializeField] float smooth;

    float shakeStrength; float shakeSpeed; float shakeSmooth; float targetFov; float changeSpeed; bool changeFov;

    Camera cam;

    [HideInInspector] public bool shake; [HideInInspector] public float fov;

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
       
        transform.parent.Rotate(transform.parent.up * mouseX);

        Vector3 rot = transform.rotation.eulerAngles;
        rot.x += -mouseY;

        if (rot.x > 180)
        {
            rot.x = rot.x < 270f ? 270f : rot.x;
        }
        else
        {
            rot.x = rot.x > 90f ? 90f : rot.x;
        }

        transform.rotation = Quaternion.Euler(rot);
    }

    void MovementTilt()
    {
        float tiltAroundX = Input.GetAxis("Horizontal") * tiltAngle;
        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -tiltAroundX);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

    public void FireRecoil(float recoil)
    {
        float verticalAngle = transform.eulerAngles.x - recoil * Random.Range(0.25f, 1);
        Quaternion target = Quaternion.Euler(verticalAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = target;
    }

    void Shake()
    {
        if (shake)
        {
            shakeStrength = Mathf.Clamp(shakeStrength, 0f, 50f);
            float randomZ = Random.value - 0.5f;

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + randomZ * shakeStrength);
            shakeStrength = Mathf.SmoothDamp(shakeStrength, 0f, ref shakeSpeed, shakeSmooth);
            shakeStrength = Mathf.Clamp(shakeStrength, 0f, 50f);

            if (shakeStrength <= 0.1f)
            {
                shake = false;
            }
        }
    }

    void FovModifier()
    {
        if (changeFov)
        {
            cam.fieldOfView += changeSpeed * Time.deltaTime;

            switch (changeSpeed > 0)
            {
                case true: // Positive

                    if (cam.fieldOfView >= targetFov)
                    {
                        cam.fieldOfView = targetFov;
                        changeFov = false;
                    }

                    break;

                case false: // Negative

                    if (cam.fieldOfView <= targetFov)
                    {
                        cam.fieldOfView = targetFov;
                        changeFov = false;
                    }

                    break;
            }           
        }       
    }

    public void ChangeFov(float targetFov, float changeSpeed)
    {
        if (!changeFov)
        {
            this.targetFov = targetFov;
            this.changeSpeed = changeSpeed;
            changeFov = true;
        }       
    }

    public void StopChangeFov()
    {
        changeFov = false;
    }

    public void StartShake(float strength, float initialSpeed, float smoothTime)
    {
        shake = false;

        shakeStrength = strength;
        shakeSpeed = initialSpeed;
        shakeSmooth = smoothTime;

        shake = true;
    }

    void Update()
    {
        MouseLook();
        MovementTilt();
        FovModifier();
        Shake();
    }

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();
        fov = cam.fieldOfView;
    }
}
