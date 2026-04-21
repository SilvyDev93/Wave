using TMPro;
using UnityEngine;

public class OptionsArrayTextHandler : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] string[] optionsStringArray;
    [SerializeField] int[] optionsTextSize;
    [SerializeField] string saveDataString;

    [Header("References")]
    [SerializeField] TextMeshProUGUI arrayTMP;

    int index = 0;

    public void TextHandling()
    {
        index += 1;

        PlayerPrefs.SetInt(saveDataString, index);
        PlayerPrefs.Save();

        if (index > optionsStringArray.Length - 1) 
        {
            index = 0;
        }

        arrayTMP.text = optionsStringArray[index];
        arrayTMP.fontSize = optionsTextSize[index];
    }

    void OnEnable()
    {
        int savedDifficulty = PlayerPrefs.GetInt(saveDataString);

        if (savedDifficulty < optionsStringArray.Length) 
        {
            arrayTMP.text = optionsStringArray[savedDifficulty];
            arrayTMP.fontSize = optionsTextSize[savedDifficulty];
        }
        else
        {
            arrayTMP.text = optionsStringArray[0];
            arrayTMP.fontSize = optionsTextSize[0];
        }        
    }
}
