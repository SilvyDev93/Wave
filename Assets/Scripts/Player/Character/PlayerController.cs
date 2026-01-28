using System.Collections;
using System.ComponentModel;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpGroundCheckDelay;
    [SerializeField] float collisionDistance;

    [Header("Gravity")]   
    [SerializeField] float fallSpeed;
    [SerializeField] float gravityIncreaseSpeed;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;
    public bool gravityEnabled = true;

    [Header("Slope Control")]
    [SerializeField] float slopeDistance;
    [SerializeField] float slopeYDistance;
    [SerializeField] float slopeLimit;

    [Header("References")]
    [SerializeField] Transform groundCheck;
    public Transform characterDirection;

    [Header("Debug")]
    [SerializeField] bool showCheckSphere;
    [SerializeField] bool showCheckRay;

    float movementSpeed; float currentGravity = 0;

    bool canJump; bool coyoting; bool wontCheckGround; bool onAir;

    Vector3 inputMovement;

    PlayerCharacter character; PlayerInput input;

    [HideInInspector] public Rigidbody rb;

    public void MovePlayer() // Player inputs handled in FixedUpdate
    {
        rb.MovePosition(transform.position + (input.GetVerticalAxis() + input.GetHorizontalAxis()) * movementSpeed * Time.fixedDeltaTime);               
    }

    void PlayerGravity()
    {
        if (!OnGround() && gravityEnabled)
        {
            onAir = true;
            currentGravity += gravityIncreaseSpeed;
            Vector3 gravity = GameManager.Instance.globalGravity * fallSpeed * currentGravity * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
            StartCoroutine(OnGroundDelayed());
        }

        if (OnGround())
        {
            currentGravity = 0;
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0; // si se jode en y es por esto

            if (!IsPlayerMoving())
            {
                velocity.x = 0;
                velocity.z = 0;
            }

            rb.linearVelocity = velocity;

            if (onAir)
            {
                //GameManager.Instance.audioManager.playerSounds.PlayLandingSound();
                PlayerSounds playerSounds = GameManager.Instance.audioManager.playerSounds;
                playerSounds.PlayAudio("landing");
            }

            onAir = false;
            canJump = true;
        }
    }

    public void Jump()
    {
        if (canJump)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0;
            rb.linearVelocity = velocity;
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
            StartCoroutine(DelayGroundCheck());
            canJump = false;
        }
    }

    IEnumerator DelayGroundCheck()
    {
        wontCheckGround = true;
        yield return new WaitForSeconds(jumpGroundCheckDelay);
        wontCheckGround = false;
    }

    public bool OnGround()
    {
        if (!wontCheckGround)
        {
            return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        }
        else
        {
            return false;
        }
    }

    IEnumerator OnGroundDelayed()
    {
        if (!coyoting)
        {
            coyoting = true;
            yield return new WaitForSeconds(coyoteTime);
            canJump = false;
            coyoting = false;
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

    void PlayerSlopeControl()
    {
        RaycastHit hit;

        Vector3 pos = groundCheck.transform.position;
        pos.y += slopeYDistance;

        if (Physics.Raycast(pos, -transform.up, out hit, slopeDistance, groundMask))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle < slopeLimit)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                characterDirection.rotation = targetRotation;
            }
        }
    }

    void FixedUpdate()
    {      
        PlayerGravity();
        PlayerSlopeControl();
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

        if (showCheckRay)
        {
            Gizmos.color = Color.red;
            Vector3 pos = groundCheck.transform.position;
            pos.y += slopeYDistance;
            Gizmos.DrawRay(pos, -transform.up * slopeDistance);
        }
    }
}
