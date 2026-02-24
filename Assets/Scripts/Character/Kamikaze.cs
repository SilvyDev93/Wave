using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Kamikaze : EnemyBehavior
{
    [SerializeField] GameObject explosion;
    [SerializeField] float detonationTime;

    public override IEnumerator InitiateAttack()
    {
        CharacterNavigation charNav = GetComponent<CharacterNavigation>();

        charNav.FollowPlayer(false);
        charNav.SetDestination(transform.position);

        yield return new WaitForSeconds(detonationTime);

        Explode();
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }

    public void OnDeath()
    {
        Explode();
    }
}
