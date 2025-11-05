using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float kickDamage;
    [SerializeField] float kickRadius;
    [SerializeField] float kickKnockback;

    [Header("References")]
    [SerializeField] Transform kickOrigin;
    [SerializeField] LayerMask entityLayer;

    public void Kick()
    {
        Collider[] hitColliders = Physics.OverlapSphere(kickOrigin.position, kickRadius, entityLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag != "Player")
            {
                hitCollider.SendMessage("TakeDamage", kickDamage);
                hitCollider.SendMessage("TakeKnockback", kickKnockback);
            }           
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(kickOrigin.position, kickRadius);
    }
}
