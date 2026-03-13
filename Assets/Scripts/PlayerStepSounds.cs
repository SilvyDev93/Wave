using System.Collections;
using UnityEngine;

public class PlayerStepSounds : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float timeBetweenSteps;

    [Header("Audio Source")]
    [SerializeField] AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip[] stepSounds;

    AudioManager audioManager;
    PlayerController controller;
    PlayerAbilities abilities;

    IEnumerator StepSoundHandler()
    {
        if (controller.IsPlayerMoving() && controller.OnGround() && !abilities.IsDashing())
        {
            audioSource.clip = stepSounds[Random.Range(0, stepSounds.Length)];
            audioManager.PlayAudio(audioSource);            
        }

        yield return new WaitForSeconds(timeBetweenSteps);
        StartCoroutine(StepSoundHandler());
    }

    IEnumerator OnAwake()
    {
        yield return new WaitForSeconds(timeBetweenSteps);

        audioManager = GameManager.Instance.audioManager;
        controller = GameManager.Instance.playerController;
        abilities = GameManager.Instance.playerAbilities;

        StartCoroutine(StepSoundHandler());
    }

    void Awake()
    {
        StartCoroutine(OnAwake());
    }
}
