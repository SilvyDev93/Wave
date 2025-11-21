using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Melee Enemy")]
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField] float time;
    [SerializeField] float duration;
    [SerializeField] float cooldown;
    [SerializeField] EnemyAttackHitbox hitbox;

    bool attacking;

    CharacterNavigation characterNavigation;
    Transform playerTransform;
    
    IEnumerator InitiateAttack()
    {
        attacking = true;

        characterNavigation.FollowPlayer(false);
        characterNavigation.SetAgentDestination(transform.position);

        yield return new WaitForSeconds(time);

        hitbox.SetHitboxActive(damage);

        yield return new WaitForSeconds(duration);

        hitbox.gameObject.SetActive(false);
        characterNavigation.FollowPlayer(true);

        yield return new WaitForSeconds(cooldown);

        attacking = false;       
    }

    void AttackRange()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= range && !attacking)
        {
            StartCoroutine(InitiateAttack());
        }
    }

    void Update()
    {
        AttackRange();
    }

    void Awake()
    {
        characterNavigation = GetComponent<CharacterNavigation>();
        playerTransform = GameManager.Instance.playerCharacter.transform;
        hitbox.gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, range);
    }
}
