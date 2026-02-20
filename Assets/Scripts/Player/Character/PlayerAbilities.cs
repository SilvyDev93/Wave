using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] float dashStrength;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration;
    [SerializeField] float dashStaminaCost;
    [SerializeField] float dashFOVspeed;
    [SerializeField] bool canDash = true;

    [Header("Kick")]
    [SerializeField] float kickDamage;
    [SerializeField] float kickRadius;
    [SerializeField] float kickKnockback;
    [SerializeField] float kickStaminaCost;

    [Header("Stomp")]
    [SerializeField] float stompVelocity;    
    [SerializeField] float stompCooldown;
    [SerializeField] float stompExplosionStrength;
    [SerializeField] float stompExplosionTime;

    [Header("References")]
    [SerializeField] Transform kickOrigin;
    [SerializeField] GameObject explosion;
    [SerializeField] PlayerSpeedLimiter playerSpeedLimiter;
    [SerializeField] LayerMask entityLayer;

    PlayerCharacter playerCharacter;
    PlayerController playerController;
    PlayerInput playerInput;
    FirstPersonCamera firstPersonCamera;

    float stompTime;

    bool stomping;

    public void Dash()
    {
        if (canDash && playerController.IsPlayerMoving() && !playerCharacter.exhausted)
        {
            StartCoroutine(PlayerDashState());
        }

        IEnumerator PlayerDashState()
        {
            canDash = false;

            PlayerSounds playerSounds = GameManager.Instance.audioManager.playerSounds;
            playerSounds.PlayAudio("dashing");

            GameManager.Instance.playerInput.lockedInput = true;
            GameManager.Instance.playerInput.lockedMovement = true;
            playerSpeedLimiter.triggerActive = false;

            firstPersonCamera.StopChangeFov();
            firstPersonCamera.ChangeFov(firstPersonCamera.fov + 20, 100);

            playerController.gravityEnabled = false;

            playerController.GetComponent<Rigidbody>().AddForce((playerInput.GetVerticalInput() + playerInput.GetHorizontalInput()) * dashStrength, ForceMode.Impulse);

            playerCharacter.ConsumeStamina(dashStaminaCost);

            yield return new WaitForSeconds(dashDuration);

            GameManager.Instance.playerInput.lockedInput = false;
            GameManager.Instance.playerInput.lockedMovement = false;
            playerSpeedLimiter.triggerActive = true;

            firstPersonCamera.StopChangeFov();
            firstPersonCamera.ChangeFov(firstPersonCamera.fov, -100);

            playerController.gravityEnabled = true;

            StartCoroutine(DashCooldown());
        }

        IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashDuration);
            canDash = true;
        }
    }

    public void Kick()
    {
        if (!playerCharacter.exhausted) 
        {
            Collider[] hitColliders = Physics.OverlapSphere(kickOrigin.position, kickRadius, entityLayer);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag != "Player")
                {
                    CharacterNPC character = hitCollider.gameObject.GetComponent<CharacterNPC>();

                    if (character != null) 
                    {
                        character.TakeDamage(kickDamage);
                        character.TakeKnockback(kickKnockback, Camera.main.transform.forward);
                    }                   
                }
            }

            playerCharacter.ConsumeStamina(kickStaminaCost);
        }        
    }

    public void Stomp()
    {
        if (!playerController.OnGround())
        {
            playerController.GetComponent<Rigidbody>().AddForce(-transform.up * stompVelocity, ForceMode.Impulse);
            stompTime = 0;
            stomping = true;
        }
    }

    void InStompDuration()
    {
        if (stomping)
        {
            stompTime += stompExplosionTime * Time.deltaTime;

            if (playerController.OnGround())
            {
                if (stompTime >= 0.8f)
                {
                    GameObject knockbackRadius = Instantiate(explosion, transform.position, explosion.transform.rotation);
                    knockbackRadius.GetComponent<Explosion>().targetSize = stompTime * stompExplosionStrength;
                    Debug.Log(stompTime);
                }
                
                stomping = false;
            }
        }      
    }

    private void Update()
    {
        InStompDuration();
    }

    private void Start()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        playerController = GetComponent<PlayerController>();
        playerInput = GameManager.Instance.playerInput;
        firstPersonCamera = Camera.main.GetComponent<FirstPersonCamera>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(kickOrigin.position, kickRadius);
    }
}
