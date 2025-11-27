using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float health;

    [Header("Stamina")]
    [SerializeField] float stamina;
    [SerializeField] float staminaRegen;
    [SerializeField] float staminaCooldown;

    public int money;

    float currentHealth; float currentStamina; bool regenStamina;

    PlayerController controller; PlayerHUD hud; Rigidbody rb;

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

        hud.SetStaminaSliderValue(currentStamina);
    }

    public void GetMoney(int amount)
    {
        money += amount;
        money = Mathf.Clamp(money, 0, 999999);
        hud.moneyCounter.text = money.ToString();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        hud.SetHealthValue((int) currentHealth);

        if (currentHealth <= 0)
        {
            KillEntity();
        }
    }

    public void RecoverHealth(int healthRecovery)
    {
        currentHealth += healthRecovery;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        hud.SetHealthValue((int) currentHealth);
    }

    public void KillEntity()
    {
        SceneManager.LoadScene(1);
    }

    public void ConsumeStamina(float consumption)
    {
        regenStamina = false;

        currentStamina -= consumption;
        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);

        hud.SetStaminaSliderValue(currentStamina);
        hud.SetStaminaSliderActive(true);

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
        rb = GetComponent<Rigidbody>();

        currentHealth = health;
        currentStamina = stamina;

        hud = GameManager.Instance.playerHUD;

        hud.healthSlider.maxValue = health;

        hud.SetHealthValue((int)currentHealth);

        hud.leftStaminaSlider.maxValue = stamina;
        hud.rightStaminaSlider.maxValue = stamina;

        regenStamina = true;

        GetMoney(0);
    }
}
