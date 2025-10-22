using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;
       
    public float movementSpeed;
  
    Rigidbody rb;
    PlayerCharacter character;

    void PlayerInputUpdate()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && !character.exhausted)
        {
            movementSpeed = runSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }
    }

    void PlayerInputFixed()
    {
        // WASD Movement
        Vector3 forward = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontal = transform.right * Input.GetAxis("Horizontal");
        rb.MovePosition(transform.position + (forward + horizontal) * movementSpeed * Time.deltaTime);
    }

    public bool IsPlayerMoving()
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

    public bool IsPlayerRunning()
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
    }

    void FixedUpdate()
    {
        PlayerInputFixed();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<PlayerCharacter>();
    }
}
