using UnityEngine;
using UnityEngine.Audio;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource landingSource;
    public AudioSource damageSource;
    public AudioSource healthRecoverySource;
    public AudioSource ammoRefillSource;
    public AudioSource dashingSource;

    public void PlayAudio(string audioName)
    {        
        switch (audioName)
        {
            case "damage":
                damageSource.Play();
                break;

            case "landing":
                landingSource.Play();
                break;

            case "healthRecovery":
                healthRecoverySource.Play();
                break;

            case "ammoRefill":
                ammoRefillSource.Play();    
                break;

            case "dashing":
                dashingSource.Play();
                break;

            default:
                Debug.Log(audioName + " does not exist.");
                break;
        }
    }
}
