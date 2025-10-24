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
        transform.Rotate(new Vector3(1, 0, 0) * -mouseY);
    }

    void MovementTilt()
    {
        float tiltAroundX = Input.GetAxis("Horizontal") * tiltAngle;
        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -tiltAroundX);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

    public void FireRecoil(float verticalRecoil, float horizontalRecoil)
    {
        float verticalAngle = transform.eulerAngles.x - verticalRecoil * Random.Range(0.25f, 1);
        //float horizontalAngle = transform.eulerAngles.y + horizontalRecoil * Random.Range(-1, 1); Glitched, fix later
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
