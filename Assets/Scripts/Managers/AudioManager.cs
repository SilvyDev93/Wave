using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void PlayAudioPitch(AudioSource audio, float newPitch)
    {
        audio.pitch = newPitch;
        audio.Play();
    }
}
