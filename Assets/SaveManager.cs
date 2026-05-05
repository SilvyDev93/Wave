using UnityEngine;
using UnityEngine.Audio;

public class SaveManager : MonoBehaviour
{
    [Header("References (Global)")]
    [SerializeField] AudioMixer mixer;

    MixerGroups groupSelection;

    enum MixerGroups
    {
        Master,
        Music,
        SFX,
        Ambience
    }

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            groupSelection = (MixerGroups) i;

            float savedVolume = PlayerPrefs.GetFloat(groupSelection.ToString());

            if (savedVolume == 0)
            {
                mixer.SetFloat(groupSelection.ToString(), 20);
            }
            else
            {
                mixer.SetFloat(groupSelection.ToString(), PlayerPrefs.GetFloat(groupSelection.ToString()));
            }
        }        
    }
}
