using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("General Audio")]
    public AudioSource sceneMusic;
    public AudioSource hitNotifier;
    public AudioSource teleport;
    public AudioSource jumpPad;

    [HideInInspector] public PlayerSounds playerSounds;
    
    public void PlayAudio(AudioSource audio)
    {
        audio.Play();
    }

    public void PlayAudioPitch(AudioSource audio, float newPitch)
    {
        audio.pitch = newPitch;
        audio.Play();
    }

    public void PlaySceneMusic(AudioClip clip)
    {
        sceneMusic.clip = clip;
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
