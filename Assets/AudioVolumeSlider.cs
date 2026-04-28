using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] MixerGroups groupSelection;

    [Header("References (Global)")]
    [SerializeField] AudioMixer mixer;

    [Header("References (Local)")]
    [SerializeField] TextMeshProUGUI titleTMP;
    [SerializeField] TextMeshProUGUI percentageTMP;   
    [SerializeField] Slider slider;
    
    float value;

    enum MixerGroups
    {
        Master,
        Music,
        SFX,
        Ambience
    }

    public void VolumeValueChange()
    {
        mixer.SetFloat(groupSelection.ToString(), slider.value);
        PlayerPrefs.SetFloat(groupSelection.ToString(), slider.value);
        PercentageDisplay();
    }

    void SliderSetUp()
    {
        titleTMP.text = groupSelection.ToString();

        slider.minValue = -80;
        slider.maxValue = 20;

        float savedVolume = PlayerPrefs.GetFloat(groupSelection.ToString());

        if (savedVolume == 0)
        {
            mixer.SetFloat(groupSelection.ToString(), 20);
        }
        else
        {
            mixer.SetFloat(groupSelection.ToString(), PlayerPrefs.GetFloat(groupSelection.ToString()));
        }

        mixer.GetFloat(groupSelection.ToString(), out value);
        slider.value = value;
        PercentageDisplay();
    }

    void PercentageDisplay()
    {
        percentageTMP.text = (slider.value + 80).ToString() + "%";
    }

    private void Awake()
    {
        SliderSetUp();
    }
}
