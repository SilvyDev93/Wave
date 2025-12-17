using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    [Header("Health Orb")]
    [SerializeField] int healthFactor;

    [Header("Animation")]
    [SerializeField] float animSpeed;
    [SerializeField] float animLenght;

    float initialY;

    void Animation()
    {
        float y = Mathf.PingPong(initialY + (Time.time * animSpeed), animLenght);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    void PlayerContact(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerCharacter>().RecoverHealth(healthFactor);
            PlayerSounds playerSounds = GameManager.Instance.audioManager.playerSounds;
            playerSounds.PlayAudio(playerSounds.healthPickUp);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Animation();
    }

    private void Start()
    {
        initialY = transform.position.y;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerContact(other);
    }
}
