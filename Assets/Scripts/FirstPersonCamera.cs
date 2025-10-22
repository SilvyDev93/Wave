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
        transform.parent.Rotate(new Vector3(0, 1, 0) * mouseX);
        transform.Rotate(new Vector3(1, 0, 0) * -mouseY);
    }

    void MovementTilt()
    {
        float tiltAroundX = Input.GetAxis("Horizontal") * tiltAngle;
        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -tiltAroundX);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

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
