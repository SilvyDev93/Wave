using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Melee Enemy")]
    public float range;
    public float time;
    public float duration;
    public float cooldown;

    [HideInInspector] public bool attacking;
    [HideInInspector] public CharacterNavigation characterNavigation;
    [HideInInspector] public Transform playerTransform;
    
    public virtual IEnumerator InitiateAttack()
    {
        yield return null;
        // Set to Attack
    }

    void AttackRange()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= range && !attacking)
        {
            StartCoroutine(InitiateAttack());
        }
    }

    public virtual void OnAwake()
    {
        characterNavigation = GetComponent<CharacterNavigation>();
        playerTransform = GameManager.Instance.playerCharacter.transform;
    }

    void Update()
    {
        AttackRange();
    }

    void Awake()
    {
        OnAwake();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, range);
    }
}
