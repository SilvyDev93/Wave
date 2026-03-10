using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player")]

    [Header("Sliders")]    
    public Transform staminaSliders;

    [Header("Counters")]
    public TextMeshProUGUI healthCounter;
    public TextMeshProUGUI moneyCounter;

    [Header("Waves")]
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI enemyCounter;

    [Header("Captions")]
    public TextMeshProUGUI shopCaption;
    public TextMeshProUGUI playerMessage;

    [Header("Hit Marker")]
    public GameObject hitMarker;
    [SerializeField] float markerLifetime;

    [Header("Screen Effects")]
    public RawImage bloodVignette;

    [Header("Other")]
    public Transform weaponSlots;
    public GameObject reloadText;
    public GameObject deathScreen;
    
    [HideInInspector] public Slider leftStaminaSlider;
    [HideInInspector] public Slider rightStaminaSlider;

    float playerMessageTime; bool emptyMessage;

    public void ReduceEnemyCounter()
    {
        GameManager.Instance.waveManager.enemyCounterNumber -= 1;
        enemyCounter.text = (GameManager.Instance.waveManager.enemyCounterNumber + " Remaining").ToString();
    }

    public void SetHealthValue(int value)
    {
        healthCounter.text = value.ToString();

        float oppositeValue = Mathf.Abs(GameManager.Instance.playerCharacter.health - value) - 30;
        float finalValue = Mathf.Clamp(oppositeValue, 0, oppositeValue);
        float transparency = (finalValue * 1) / GameManager.Instance.playerCharacter.health;

        Color alpha = bloodVignette.color;
        alpha.a = transparency;
        bloodVignette.color = alpha;
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

    public void SetPlayerMessage(string text, float time)
    {
        playerMessage.text = text;
        playerMessageTime = time;
        emptyMessage = true;
    }

    void EmptyPlayerMessage()
    {
        if (emptyMessage)
        {
            if (playerMessageTime <= 0)
            {
                playerMessage.text = "";
                emptyMessage = false;
            }
            else
            {
                playerMessageTime -= 1 * Time.deltaTime;
            }
        }
    }

    void Update()
    {
        HideStamina();
        EmptyPlayerMessage();
    }

    void Awake()
    {
        leftStaminaSlider = staminaSliders.GetChild(0).GetComponent<Slider>();
        rightStaminaSlider = staminaSliders.GetChild(1).GetComponent<Slider>();

        deathScreen.SetActive(false);
        hitMarker.SetActive(false);
    }
}
