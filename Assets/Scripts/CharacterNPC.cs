using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterNPC : MonoBehaviour
{
    [Header("Character Parameters")]
    [SerializeField] float health;
    [SerializeField] float movementSpeed;
    [SerializeField] int reward;
    [SerializeField] float healthBarTime;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

    [Header("References")]  
    [SerializeField] Slider healthSlider;
    [SerializeField] GameObject corpse;

    Rigidbody rb; CharacterNavigation characterNavigation;

    float currentHealth; bool groundCheckEnabled = true;

    [HideInInspector] public bool onExplosionCooldown;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        healthSlider.value = currentHealth;

        StopCoroutine(HideHealthBar());
        healthSlider.gameObject.SetActive(true);

        if (currentHealth <= 0)
        {           
            KillEntity();
        }

        StartCoroutine(HideHealthBar());
    }

    public void TakeKnockback(float force, Vector3 direction)
    {
        SeparateFromGround();

        rb.AddForce(force * direction, ForceMode.Impulse);
        rb.AddForce(force * transform.up, ForceMode.Impulse); // quitar
    }

    public void SeparateFromGround()
    {
        rb.isKinematic = false;
        characterNavigation.SetAgentActive(false);
        groundCheckEnabled = false;
    }

    public void KillEntity()
    {
        GameManager.Instance.playerCharacter.GetMoney(reward);
        GameManager.Instance.playerHUD.ReduceEnemyCounter();
        GameObject corpseObject = Instantiate(corpse, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public bool OnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }

    public IEnumerator ExplosionDamageCooldown(float cooldown)
    {
        onExplosionCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onExplosionCooldown = false;
    }

    IEnumerator GroundCheckCoroutine()
    {
        switch (groundCheckEnabled)
        {
            case true:

                if (OnGround())
                {
                    rb.isKinematic = true;
                    characterNavigation.SetAgentActive(true);
                }
                
                break;

            case false:
                StartCoroutine(GroundCheckWait());
                break;

                IEnumerator GroundCheckWait()
                {
                    yield return new WaitForSeconds(0.1f);
                    groundCheckEnabled = true;
                }
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(GroundCheckCoroutine());        
    }

    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(healthBarTime);
        healthSlider.gameObject.SetActive(false);
    }

    void GetReferences()
    {
        currentHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = currentHealth;
        healthSlider.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        characterNavigation = GetComponent<CharacterNavigation>();
    }

    void SetCharacterParameters()
    {
        characterNavigation.agent.speed = movementSpeed;
    }

    void Start()
    {
        GetReferences();
        SetCharacterParameters();
        StartCoroutine(GroundCheckCoroutine());
    }
}
