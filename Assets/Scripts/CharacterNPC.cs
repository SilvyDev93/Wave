using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNPC : MonoBehaviour
{
    [Header("Character Parameters")]
    [SerializeField] float health;
    [SerializeField] int reward;
    
    [Header("References")]
    [SerializeField] Slider healthSlider;

    Rigidbody rb; CharacterNavigation characterNavigation;

    float currentHealth;

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
        characterNavigation.DisableAgent();
        rb.AddForce(force * Camera.main.transform.forward, ForceMode.Impulse);
        rb.AddForce(force * transform.up, ForceMode.Impulse);
    }

    public void KillEntity()
    {
        GameManager.Instance.playerCharacter.GetMoney(reward);
        GameManager.Instance.playerHUD.ReduceEnemyCounter();
        Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = currentHealth;
        healthSlider.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        characterNavigation = GetComponent<CharacterNavigation>();
    }
}
