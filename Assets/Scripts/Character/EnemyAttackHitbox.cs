using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    int damage;

    public void SetHitboxActive(int damage)
    {
        this.damage = damage;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerCharacter>().TakeDamage(damage);
        }
    }
}
