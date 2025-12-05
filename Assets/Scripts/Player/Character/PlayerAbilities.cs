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

    [Header("References")]
    [SerializeField] Transform kickOrigin;
    [SerializeField] LayerMask entityLayer;

    PlayerCharacter playerCharacter;
    PlayerController playerController;
    PlayerInput playerInput;
    FirstPersonCamera firstPersonCamera;

    public void Dash()
    {
        if (canDash && playerController.IsPlayerMoving() && !playerCharacter.exhausted)
        {
            StartCoroutine(PlayerDashState());
        }

        IEnumerator PlayerDashState()
        {
            canDash = false;

            GameManager.Instance.playerInput.lockedInput = true;

            firstPersonCamera.StopChangeFov();
            firstPersonCamera.ChangeFov(firstPersonCamera.fov + 20, 100);

            playerController.gravityEnabled = false;

            playerController.GetComponent<Rigidbody>().AddForce((playerInput.GetVerticalAxis().normalized + playerInput.GetHorizontalAxis().normalized) * dashStrength, ForceMode.Impulse);

            playerCharacter.ConsumeStamina(dashStaminaCost);

            yield return new WaitForSeconds(dashDuration);

            GameManager.Instance.playerInput.lockedInput = false;

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
                        //hitCollider.SendMessage("TakeDamage", kickDamage);
                        //hitCollider.SendMessage("TakeKnockback", kickKnockback);
                        character.TakeDamage(kickDamage);
                        character.TakeKnockback(kickKnockback, Camera.main.transform.forward);
                    }                   
                }
            }

            playerCharacter.ConsumeStamina(kickStaminaCost);
        }        
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
