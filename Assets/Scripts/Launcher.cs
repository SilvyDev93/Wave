using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    [Header("Dropdowns")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] TMP_Dropdown fpsDropdown;

    [Header("Toggles")]
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Toggle vsyncToggle;

    Resolution[] loadedResolutions;

    public void Play() // Used by Play button
    {
        SaveParameters();
        SceneManager.LoadScene(1);
    }

    public void Exit() // Used by Quit button
    {
        Application.Quit();
    }

    void SetScreen() // Sets the launcher's window mode and size
    {
        Screen.SetResolution(455, 455, false);
    }

    void SetDropdowns() // Add neccesary options to their corresponding dropdown
    {
        // Resolution Dropdown

        Resolution[] resolutions = Screen.resolutions; // Get all avaiable resolutions
        loadedResolutions = new Resolution[resolutions.Length]; // Create an array to store all resolutions that end up being loaded

        List<string> resolutionList = new List<string>(); // List to store resolution strings
        resolutionList.Add(""); // Add empty option to later fill with current/saved resolution

        int i = 1;

        foreach (var res in resolutions)
        {
            if (res.refreshRateRatio.value == 30 || res.refreshRateRatio.value == 60 || res.refreshRateRatio.value == 120)
            {
                resolutionList.Add(res.width + "x" + res.height + " : " + res.refreshRateRatio);
                loadedResolutions[i] = res;
                i++;
            }
        }

        resolutionDropdown.AddOptions(resolutionList); // Add all resolution strings in list to the dropdown


        // Quality Dropdown

        qualityDropdown.AddOptions(QualitySettings.names.ToList()); // Get all quality levels created in project
    }

    void SetOptions() // Either do a function or the other depending if it is the first time starting the game
    {
        switch (PlayerPrefs.GetInt("AlreadyLaunched"))
        {
            case 1:
                SetSavedOptions();
                break;

            default:
                SetDefaultOptions();
                break;
        }
    }

    void SetSavedOptions() // Set saved configuration when launching
    {
        resolutionDropdown.options[0].text = PlayerPrefs.GetInt("ResolutionWidth") + "x" + PlayerPrefs.GetInt("ResolutionHeight") + " : " + PlayerPrefs.GetInt("RefreshRateRatio");
        qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel");
        fpsDropdown.captionText.text = PlayerPrefs.GetInt("TargetFPS").ToString();
        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
        vsyncToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Vsync"));
    }

    void SetDefaultOptions() // Set default configurations when launching for the first time
    {
        Resolution res = Screen.currentResolution; resolutionDropdown.options[0].text = res.width + "x" + res.height + " : " + res.refreshRateRatio;
        qualityDropdown.value = qualityDropdown.options.Count - 1;        
        fpsDropdown.value = 1;
        SaveParameters();
    }

    void SaveParameters() // Save given user parameters when closing the launcher
    {
        PlayerPrefs.SetInt("ResolutionWidth", loadedResolutions[resolutionDropdown.value].width);
        PlayerPrefs.SetInt("ResolutionHeight", loadedResolutions[resolutionDropdown.value].height);
        PlayerPrefs.SetInt("RefreshRateRatio", (int)loadedResolutions[resolutionDropdown.value].refreshRateRatio.value);
        PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);
        PlayerPrefs.SetInt("TargetFPS", Convert.ToInt32(fpsDropdown.options[fpsDropdown.value].text));        
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(fullscreenToggle.isOn));
        PlayerPrefs.SetInt("Vsync", Convert.ToInt32(vsyncToggle.isOn));
        PlayerPrefs.SetInt("AlreadyLaunched", 1);
        PlayerPrefs.Save();
    }

    void Awake()
    {
        SetScreen(); 
        SetDropdowns();
        SetOptions();
    }

    void OnApplicationQuit()
    {
        SaveParameters();
    }
}
