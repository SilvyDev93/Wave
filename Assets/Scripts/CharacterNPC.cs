using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNPC : MonoBehaviour
{
    [Header("Character Parameters")]
    [SerializeField] float health;
    [SerializeField] int reward;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

    [Header("References")]  
    [SerializeField] Slider healthSlider;

    Rigidbody rb; CharacterNavigation characterNavigation;

    float currentHealth; bool groundCheckEnabled = true;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        healthSlider.value = currentHealth;
        healthSlider.gameObject.SetActive(true);

        if (currentHealth <= 0)
        {           
            KillEntity();
        }
    }

    public void TakeKnockback(float force)
    {
        rb.isKinematic = false;
        characterNavigation.SetAgentActive(false);
        groundCheckEnabled = false;

        rb.AddForce(force * Camera.main.transform.forward, ForceMode.Impulse);
        rb.AddForce(force * transform.up, ForceMode.Impulse);
    }

    public void KillEntity()
    {
        GameManager.Instance.playerCharacter.GetMoney(reward);
        GameManager.Instance.playerHUD.ReduceEnemyCounter();
        Destroy(gameObject);
    }

    public bool OnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
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

    void Start()
    {
        currentHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = currentHealth;
        healthSlider.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        characterNavigation = GetComponent<CharacterNavigation>();
        StartCoroutine(GroundCheckCoroutine());
    }
}
