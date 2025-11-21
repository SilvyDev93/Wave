using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    public void TakeDamage(float damage)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
