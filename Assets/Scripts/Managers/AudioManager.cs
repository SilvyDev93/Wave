using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("General Audio")]
    public AudioSource hitNotifier;

    public void PlayAudioPitch(AudioSource audio, float newPitch)
    {
        audio.pitch = newPitch;
        audio.Play();
    }
}
