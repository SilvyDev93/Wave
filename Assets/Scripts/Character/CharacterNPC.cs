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
    [SerializeField] float explosionCooldown;
    [SerializeField] DeathMode deathMode;

    [Header("Damage Numbers")]
    [SerializeField] GameObject damageNumbers;

    [Header("Explosion Death")]
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject bloodDecal;

    [Header("Drops")]
    [SerializeField] float dropChance;
    [SerializeField] GameObject drop;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

    [Header("References")]  
    [SerializeField] Slider healthSlider;
    [SerializeField] GameObject corpse;
    [SerializeField] Transform damageNumbersTransform;

    Rigidbody rb; CharacterNavigation characterNavigation;

    float currentHealth; bool groundCheckEnabled = true;

    public bool onExplosionCooldown;

    Vector3 hitPoint; Vector3 hitDirection; float hitStrenght;

    public enum DeathMode
    {
        Corpse,
        Explosion
    }

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

        GameObject damageNumber = Instantiate(damageNumbers, damageNumbersTransform);
        damageNumber.GetComponent<DamageNumber>().SetDamageNumber((int) damage);
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

        switch (deathMode)
        {
            case DeathMode.Corpse:                
                GameObject corpseObject = Instantiate(corpse, transform.position, transform.rotation);
                corpseObject.GetComponent<CorpseObject>().PushCorpse(hitPoint, hitDirection, hitStrenght);
                break;

            case DeathMode.Explosion:
                //GameObject explosion = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), transform.position, transform.rotation); //Explosion effect
                Vector3 pos = transform.position;
                pos.y -= 1;
                Instantiate(bloodDecal, pos, bloodDecal.transform.rotation); // blood decal
                break;
        }
        
        DropItem();
        Destroy(gameObject);
    }

    public void SetLastHitPush(Vector3 point, Vector3 direction, float push)
    {
        hitPoint = point;
        hitDirection = direction;
        hitStrenght = push;
    }

    void DropItem()
    {
        if (GameManager.Instance.ProbabilityCalculation(dropChance))
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }

    public bool OnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    }

    public IEnumerator ExplosionDamageCooldown()
    {
        onExplosionCooldown = true;
        yield return new WaitForSeconds(explosionCooldown);
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

    private void Update()
    {        
        if (onExplosionCooldown)
        {
            StartCoroutine(ExplosionDamageCooldown());
        }
    }

    void Start()
    {
        GetReferences();
        SetCharacterParameters();
        StartCoroutine(GroundCheckCoroutine());
    }

    private void OnDestroy()
    {
        damageNumbersTransform.SetParent(null);
        damageNumbersTransform.GetComponent<DestroyAfterTime>().StartObjectDestruction();
    }
}
