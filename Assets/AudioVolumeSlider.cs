using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI titleTMP;
    [SerializeField] TextMeshProUGUI percentageTMP;
    [SerializeField] MixerGroups groupSelection;

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
        PercentageDisplay();
    }

    void SliderSetUp()
    {
        titleTMP.text = groupSelection.ToString();
        slider.minValue = -80;
        slider.maxValue = 20;
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
