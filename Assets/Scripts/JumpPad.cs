using System.Collections;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float impulseForce;
    [SerializeField] float audioCooldown;

    bool onAudioCooldown;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.playerController.StopGravityInfluence();

            if (!onAudioCooldown)
            {
                AudioManager audioManager = GameManager.Instance.audioManager;
                audioManager.PlayAudio(audioManager.jumpPad);
                StartCoroutine(AudioCooldown());
            }

            Vector2 force = new Vector2(0, impulseForce);
            other.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }

    IEnumerator AudioCooldown()
    {
        onAudioCooldown = true;
        yield return new WaitForSeconds(audioCooldown);
        onAudioCooldown = false;
    }
}
