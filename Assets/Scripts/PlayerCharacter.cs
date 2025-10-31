using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float health;

    [Header("Stamina")]
    [SerializeField] float stamina;
    [SerializeField] float staminaRegen;
    [SerializeField] float staminaCooldown;

    [SerializeField] int money;

    float currentHealth; float currentStamina; bool regenStamina;

    PlayerController controller; PlayerHUD hud;

    [HideInInspector] public bool exhausted; [HideInInspector] public bool shopAvailable; 

    void StaminaHandling()
    {
        if (currentStamina <= 0)
        {
            exhausted = true;
            regenStamina = true;
        }

        if (currentStamina >= stamina)
        {
            exhausted = false;
        }

        if (regenStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);
        hud.staminaSlider.value = currentStamina;
    }

    public void GetMoney(int amount)
    {
        money += amount;
        hud.moneyCounter.text = money.ToString();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        hud.healthSlider.value = currentHealth;
    }

    public void ConsumeStamina(float consumption)
    {
        regenStamina = false;

        currentStamina -= consumption;
        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);
        hud.staminaSlider.value = currentStamina;

        StartCoroutine(RestartRegen());

        IEnumerator RestartRegen()
        {
            yield return new WaitForSeconds(staminaCooldown);
            regenStamina = true;
        }
    }

    private void Update()
    {
        StaminaHandling();
    }

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        currentHealth = health;
        currentStamina = stamina;

        hud = GameManager.Instance.playerHUD;

        hud.healthSlider.maxValue = health;
        hud.healthSlider.value = currentHealth;
        hud.staminaSlider.maxValue = stamina;

        regenStamina = true;

        GetMoney(0);
    }
}
