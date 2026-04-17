using UnityEngine;

public class MenuAudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource selectionSound;
    [SerializeField] AudioSource pressSound;

    public void PlaySelectionSound()
    {
        selectionSound.pitch = Random.Range(0.8f, 1);
        selectionSound.Play();
    }

    public void PlayPressSound()
    {
        pressSound.pitch = Random.Range(0.8f, 1);
        pressSound.Play();
    }
}
