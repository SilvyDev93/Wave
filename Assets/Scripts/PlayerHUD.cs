using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player")]

    [Header("Sliders")]
    public Slider healthSlider;
    public Slider staminaSlider;

    [Header("Counters")]
    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI moneyCounter;

    [Header("Waves")]
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI enemyCounter;

    public void UpdateText(string text, TextMeshProUGUI tmp)
    {
        tmp.text = text;
    }

    public void ReduceEnemyCounter()
    {
        enemyCounter.text = (int.Parse(enemyCounter.text) - 1).ToString();
    }
}
