using UnityEngine;
using UnityEngine.Audio;

public class PlayerSounds : MonoBehaviour
{
    [Header("Player Sounds")]
    public AudioClip ammoPickUp;
    public AudioClip healthPickUp;
    public AudioClip dash;

    AudioSource audioSource;

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
