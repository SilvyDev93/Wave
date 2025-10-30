using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;

    [Header("Dash")]
    [SerializeField] float dashStrength;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration;
    [SerializeField] bool canDash = true;

    [Header("Gravity")]   
    [SerializeField] float fallSpeed;   
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool gravityEnabled = true;

    [Header("References")]
    [SerializeField] Transform groundCheck;

    float movementSpeed;

    Rigidbody rb; PlayerCharacter character; PlayerInput input;

    public void Jump()
    {
        if (OnGround())
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    public void Dash()
    {
        if (canDash && !character.exhausted)
        {
            StartCoroutine(PlayerDashState());
        }

        IEnumerator PlayerDashState()
        {
            canDash = false;

            gravityEnabled = false;

            rb.AddForce(transform.forward * dashStrength, ForceMode.Impulse);

            yield return new WaitForSeconds(dashDuration);

            gravityEnabled = true;

            StartCoroutine(DashCooldown());
        }

        IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashDuration);
            canDash = true;
        }
    }

    void PlayerInputFixed() // Player inputs handled in FixedUpdate
    {
        rb.MovePosition(transform.position + (input.GetVerticalAxis() + input.GetHorizontalAxis()) * movementSpeed * Time.deltaTime);
    }

    public bool OnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }

    void PlayerGravity()
    {
        if (!OnGround() && gravityEnabled)
        {
            Vector3 gravity = GameManager.Instance.globalGravity * fallSpeed * Vector3.up;
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

    void FixedUpdate()
    {
        PlayerInputFixed();       
        PlayerGravity();
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
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
    }
}
