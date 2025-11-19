using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    [SerializeField] int healthFactor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerCharacter>().RecoverHealth(healthFactor);
            Destroy(gameObject);
        }
    }
}
