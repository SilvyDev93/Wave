using UnityEngine;
using UnityEngine.Audio;

public class PlayerSounds : MonoBehaviour
{
    [Header("Player Sounds")]
    public AudioClip ammoPickUp;
    public AudioClip healthPickUp;
    public AudioClip dash;

    [Header("Audio Sources")]
    public AudioSource landingSource;

    AudioSource audioSource;

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayLandingSound()
    {
        landingSource.Play();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
