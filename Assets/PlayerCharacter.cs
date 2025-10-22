using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float health;

    [Header("Stamina")]
    [SerializeField] float stamina;
    [SerializeField] float staminaRegen;
    [SerializeField] float sprintStaminaCost;

    [Header("References")]    
    [SerializeField] PlayerHUD hud;

    float currentHealth;
    float currentStamina;

    bool regenStamina;
    public bool exhausted;

    PlayerController controller;

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

        if (!exhausted)
        {
            if (controller.IsPlayerRunning() && controller.IsPlayerMoving())
            {
                currentStamina -= sprintStaminaCost * Time.deltaTime;
                regenStamina = false;
            }
            else
            {
                regenStamina = true;
            }
        }

        if (regenStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);
        hud.staminaSlider.value = currentStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        hud.healthSlider.value = currentHealth;
    }

    private void Update()
    {
        StaminaHandling();
    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        currentHealth = health;
        currentStamina = stamina;

        hud.healthSlider.maxValue = health;
        hud.healthSlider.value = currentHealth;
        hud.staminaSlider.maxValue = stamina;
    }
}
