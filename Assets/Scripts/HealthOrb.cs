using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    [Header("Health Orb")]
    [SerializeField] int healthFactor;

    void PlayerContact(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerCharacter>().RecoverHealth(healthFactor);
            PlayerSounds playerSounds = GameManager.Instance.audioManager.playerSounds;
            playerSounds.PlayAudio("healthRecovery");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerContact(other);
    }
}
