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

    [SerializeField] int money;

    float currentHealth; float currentStamina; bool regenStamina;

    PlayerController controller; PlayerHUD hud;

    [HideInInspector] public bool exhausted;

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

        GetMoney(0);
    }
}
