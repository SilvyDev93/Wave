using System;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] bool active;

    void ApplySettings()
    {
        //Resolution and RefreshRate
        #pragma warning disable CS0618
        if (active)
        {
            Screen.SetResolution(PlayerPrefs.GetInt("ResolutionWidth"), PlayerPrefs.GetInt("ResolutionHeight"), Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen")), PlayerPrefs.GetInt("RefreshRateRatio"));
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
            Application.targetFrameRate = PlayerPrefs.GetInt("TargetFPS");
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync");
        }
        else
        {
            Screen.SetResolution(1920, 1080, true, 60);
        }
    }

    void Awake()
    {
        ApplySettings();
    }
}
