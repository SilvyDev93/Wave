using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;

    [Header("Gravity")]   
    [SerializeField] float fallSpeed;
    [SerializeField] float gravityIncreaseSpeed;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;
    public bool gravityEnabled = true;

    [Header("Snapping")]
    [SerializeField] bool snap;
    [SerializeField] float snapDistance;
    [SerializeField] float snapOffset;

    [Header("References")]
    [SerializeField] Transform groundCheck;

    [Header("Debug")]
    [SerializeField] bool showCheckSphere;
    [SerializeField] bool showSnapRay;

    float movementSpeed; float currentGravity = 0;

    PlayerCharacter character; PlayerInput input;

    [HideInInspector] public Rigidbody rb;

    public void MovePlayer() // Player inputs handled in FixedUpdate
    {
        rb.MovePosition(transform.position + (input.GetVerticalAxis() + input.GetHorizontalAxis()) * movementSpeed * Time.deltaTime);
    }

    void PlayerGravity()
    {
        if (!OnGround() && gravityEnabled)
        {
            currentGravity += gravityIncreaseSpeed;
            Vector3 gravity = GameManager.Instance.globalGravity * fallSpeed * currentGravity * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        if (OnGround())
        {
            currentGravity = 0;
        }
    }

    void PlayerSnapping()
    {
        if (OnGround() && snap) 
        {
            Ray ray = new Ray();

            ray.origin = groundCheck.position + Vector3.up * snapOffset;
            ray.direction = -transform.up;

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, snapDistance))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                Debug.Log(slopeAngle);
                slopeAngle = Mathf.Clamp(slopeAngle, 0, 45);
                SetCharacterRotationX(-slopeAngle);
            }           
        }
    }

    void SetCharacterRotationX(float slopeAngle)
    {
        transform.localEulerAngles = new Vector3(slopeAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public void Jump()
    {
        if (OnGround())
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    public bool OnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }

    public bool IsPlayerMoving() // Used by other scripts to know if player is moving
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void FixedUpdate()
    {      
        PlayerGravity();
        PlayerSnapping();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<PlayerCharacter>();
        input = transform.parent.GetComponent<PlayerInput>();
        rb.freezeRotation = true; // If rotation is not freezed it leads to collision glitches
        movementSpeed = walkSpeed;
    }

    void OnDrawGizmosSelected()
    {
        if (showCheckSphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
        }
  
        if (showSnapRay) 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(groundCheck.position + Vector3.up * snapOffset, -transform.up * snapDistance);
        }      
    }
}
