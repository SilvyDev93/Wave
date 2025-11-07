using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] float sensitivity;

    [Header("Movement Tilt")]
    [SerializeField] float tiltAngle;
    [SerializeField] float smooth;

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

    // Move CameraShake.cs functions to here

    void Update()
    {
        MouseLook();
        MovementTilt();
    }

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
