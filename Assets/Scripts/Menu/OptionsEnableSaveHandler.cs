using TMPro;
using UnityEngine;

public class OptionsEnableSaveHandler : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] string saveDataString;

    [Header("References")]
    [SerializeField] TextMeshProUGUI enabledTMP;

    public void SaveData()
    {
        int value = 0;

        switch (enabledTMP.text)
        {
            case "On":
                value = 1;
                break;

            case "Off":
                value = 0; 
                break;
        }

        PlayerPrefs.SetInt(saveDataString, value);
        PlayerPrefs.Save();
    }

    void OnEnable()
    {
        switch (PlayerPrefs.GetInt(saveDataString))
        {
            case 0:
                enabledTMP.SetText("Off");
                break;

            case 1:
                enabledTMP.SetText("On");
                break;
        }
    }
}
