using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterNPC : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int level;

    [Header("Character Parameters (Affected by level)")]
    [SerializeField] float baseHealth;
    [SerializeField] int baseDamage;
    
    [Header("Character Parameters (Not affected by level)")]
    [SerializeField] float movementSpeed;
    [SerializeField] int reward;

    [Header("Configuration")]
    [SerializeField] float healthBarTime;
    [SerializeField] float explosionCooldown;
    [SerializeField] DeathMode deathMode;

    [Header("Billboard")]
    [SerializeField] GameObject billboard;
    [SerializeField] TextMeshProUGUI nameBillboard;
    [SerializeField] TextMeshProUGUI levelBillboard;
    [SerializeField] Slider healthSlider;

    [Header("Damage Numbers")]
    [SerializeField] GameObject damageNumbers;

    [Header("Explosion Death")]
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject bloodDecal;

    [Header("Drops")]
    [SerializeField] float dropChance;
    [SerializeField] GameObject[] drops;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

    [Header("References")]       
    [SerializeField] GameObject corpse;
    [SerializeField] Transform damageNumbersTransform;

    Rigidbody rb; CharacterNavigation characterNavigation;

    // OnGame Value
    float currentHealth;
    [HideInInspector] public float currentDamage;

    bool groundCheckEnabled = true; bool dying;

    public bool onExplosionCooldown;

    Vector3 hitPoint; Vector3 hitDirection; float hitStrenght;

    public enum DeathMode
    {
        Corpse,
        Explosion
    }

    public void RecieveDamageParameters(DamageParameters damageParameters)
    {
        bool criticalDamage = IsCrit();
        float damage = DamageCalculation();

        DamageNumbersInstantiate(damage, criticalDamage);
        SetBillboardActive();
        TakeDamage(damage);              

        bool IsCrit()
        {
            if (Random.Range(0, 101) <= damageParameters.criticalChance)
            {
                return true;
            }

            return false;
        }

        float DamageCalculation()
        {
            int damage = Random.Range(damageParameters.minDamage, damageParameters.maxDamage + 1);

            if (criticalDamage)
            {
                return damage * 2;
            }

            return damage;
        }
    }

    void SetBillboardActive()
    {
        StopCoroutine(HideBillboard());
        billboard.SetActive(true);
        StartCoroutine(HideBillboard());
    }

    void DamageNumbersInstantiate(float damage, bool isCrit)
    {
        GameObject damageNumber = Instantiate(damageNumbers, damageNumbersTransform);
        DamageNumber dmgNumber = damageNumber.GetComponent<DamageNumber>();
        dmgNumber.SetDamageNumber((int)damage);

        if (isCrit)
        {
            dmgNumber.SetTextColor(Color.yellow);
        }       
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {           
            KillEntity();
        }          
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
        if (!dying)
        {
            dying = true;

            GameManager.Instance.playerCharacter.GetMoney(reward);
            GameManager.Instance.playerHUD.ReduceEnemyCounter();

            switch (deathMode)
            {
                case DeathMode.Corpse:
                    GameObject corpseObject = Instantiate(corpse, transform.position, transform.rotation);
                    corpseObject.GetComponent<CorpseObject>().PushCorpse(hitPoint, hitDirection, hitStrenght);
                    break;

                case DeathMode.Explosion:
                    GameObject explosion = Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation); //Explosion effect
                    Vector3 pos = transform.position;
                    pos.y -= 1;
                    Instantiate(bloodDecal, pos, bloodDecal.transform.rotation); // blood decal
                    break;
            }

            DropItem();
            SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }       
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
            Instantiate(drops[Random.Range(0, drops.Length)], transform.position, Quaternion.identity);
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

    IEnumerator HideBillboard()
    {
        yield return new WaitForSeconds(healthBarTime);
        billboard.SetActive(false);
    }

    void GetReferences()
    {
        currentHealth = baseHealth;
        healthSlider.maxValue = baseHealth;
        healthSlider.value = currentHealth;
        billboard.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        characterNavigation = GetComponent<CharacterNavigation>();
    }

    void SetCharacterParametersByLevel()
    {
        currentHealth = baseHealth + ((baseHealth * 0.25f) * level);
        currentDamage = baseDamage + ((baseDamage * 0.25f) * level);
    }

    public void ChangeLevel(int newLevel)
    {
        level = newLevel;
        SetCharacterParametersByLevel();
    }

    void SetCharacterParameters()
    {
        characterNavigation.agent.speed = movementSpeed;
    }

    void SetBillboardParameters()
    {
        nameBillboard.text = characterName;
        levelBillboard.text = level.ToString();
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
        SetCharacterParametersByLevel();
        SetBillboardParameters();
        StartCoroutine(GroundCheckCoroutine());
    }

    private void OnDestroy()
    {
        damageNumbersTransform.SetParent(null);
        damageNumbersTransform.GetComponent<DestroyAfterTime>().StartObjectDestruction();
    }
}
