using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Kamikaze : EnemyBehavior
{
    [SerializeField] GameObject explosion;

    public override IEnumerator InitiateAttack()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
        yield return null;
    }

    public void OnDeath()
    {
        StartCoroutine(InitiateAttack());
    }
}
