using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;

    [Header("Gravity")]
    public bool gravity = true;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

    [Header("References")]
    [SerializeField] Transform groundCheck;

    float movementSpeed; public bool isGrounded;
  
    Rigidbody rb; PlayerCharacter character; 

    void PlayerInputUpdate() // Player inputs handled in Update
    {
        Jump();
        Sprint();

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
            }
        }

        void Sprint()
        {
            if (Input.GetKey(KeyCode.LeftShift) && !character.exhausted)
            {
                movementSpeed = runSpeed;
            }
            else
            {
                movementSpeed = walkSpeed;
            }
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }

    void PlayerInputFixed() // Player inputs handled in FixedUpdate
    {
        Vector3 forward = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontal = transform.right * Input.GetAxis("Horizontal");
        rb.MovePosition(transform.position + (forward + horizontal) * movementSpeed * Time.deltaTime);
        //rb.AddForce((forward + horizontal), ForceMode.VelocityChange);        
    }

    void PlayerGravity()
    {
        if (gravity)
        {
            Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
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

    public bool IsPlayerRunning() // Used by other scripts to know if player is running
    {
        if (movementSpeed == runSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        PlayerInputUpdate();
        GroundCheck();
        //Vector3 eulerAngles = transform.eulerAngles;
        //transform.eulerAngles = new Vector3(0, eulerAngles.y, 0);        
    }

    void FixedUpdate()
    {
        PlayerInputFixed();       
        PlayerGravity();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<PlayerCharacter>();
        rb.freezeRotation = true; // If rotation is not freezed it leads to collision glitches
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
    }
}
