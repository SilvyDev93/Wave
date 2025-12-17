using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("General Audio")]
    public AudioSource sceneMusic;
    public AudioSource hitNotifier;

    [HideInInspector] public PlayerSounds playerSounds;

    public void PlayAudioPitch(AudioSource audio, float newPitch)
    {
        audio.pitch = newPitch;
        audio.Play();
    }

    public void PlaySceneMusic()
    {
        sceneMusic.Play();
    }

    public void StopSceneMusic()
    {
        sceneMusic.Pause();
    }

    public void GetReferences()
    {
        playerSounds = GameObject.Find("PlayerSounds").GetComponent<PlayerSounds>();
    }
}
