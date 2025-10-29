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

    float currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        healthSlider.value = currentHealth;
        healthSlider.gameObject.SetActive(true);

        if (currentHealth <= 0)
        {
            GameManager.Instance.playerCharacter.GetMoney(reward);
            GameManager.Instance.playerHUD.ReduceEnemyCounter();           
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = currentHealth;
        healthSlider.gameObject.SetActive(false);
    }
}
