using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player")]

    [Header("Sliders")]
    public Slider healthSlider;    
    public Transform staminaSliders;

    [Header("Counters")]
    public TextMeshProUGUI healthCounter;
    public TextMeshProUGUI moneyCounter;

    [Header("Waves")]
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI enemyCounter;

    [Header("Captions")]
    public TextMeshProUGUI shopCaption;

    [Header("Hit Marker")]
    public GameObject hitMarker;
    [SerializeField] float markerLifetime;

    [Header("Other")]
    public Transform weaponSlots;
    public GameObject reloadText;
    public GameObject deathScreen;
    

    [HideInInspector] public Slider leftStaminaSlider;
    [HideInInspector] public Slider rightStaminaSlider;

    public void ReduceEnemyCounter()
    {
        enemyCounter.text = (int.Parse(enemyCounter.text) - 1).ToString();
    }

    public void SetHealthValue(int value)
    {
        healthCounter.text = value.ToString();
        healthSlider.value = value;
    }

    public void SetStaminaSliderValue(float stamina)
    {        
        leftStaminaSlider.value = stamina;
        rightStaminaSlider.value = stamina;
    }

    public void SetStaminaSliderActive(bool activeState)
    {
        leftStaminaSlider.gameObject.SetActive(activeState);
        rightStaminaSlider.gameObject.SetActive(activeState);
    }

    public WeaponSlot GetWeaponSlot(int index)
    {
        return weaponSlots.GetChild(index).GetComponent<WeaponSlot>();
    }

    public void HitMarkerActive()
    {
        if (!hitMarker.activeSelf)
        StartCoroutine(MarkerCoroutine());
        
        IEnumerator MarkerCoroutine()
        {
            hitMarker.SetActive(true);

            AudioManager audioManager = GameManager.Instance.audioManager;
            audioManager.PlayAudioPitch(audioManager.hitNotifier, 1);
            yield return new WaitForSeconds(markerLifetime);
            hitMarker.SetActive(false);
        }
    }

    void HideStamina()
    {
        if (leftStaminaSlider.value == leftStaminaSlider.maxValue && rightStaminaSlider.value == rightStaminaSlider.maxValue)
        {
            leftStaminaSlider.gameObject.SetActive(false);
            rightStaminaSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HideStamina();
    }

    void Awake()
    {
        leftStaminaSlider = staminaSliders.GetChild(0).GetComponent<Slider>();
        rightStaminaSlider = staminaSliders.GetChild(1).GetComponent<Slider>();

        deathScreen.SetActive(false);
        hitMarker.SetActive(false);
    }
}
