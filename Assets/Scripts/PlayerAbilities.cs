using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float kickDamage;
    [SerializeField] float kickRadius;
    [SerializeField] float kickKnockback;
    [SerializeField] float kickStaminaCost;

    [Header("References")]
    [SerializeField] Transform kickOrigin;
    [SerializeField] LayerMask entityLayer;

    PlayerCharacter character;

    public void Kick()
    {
        if (!character.exhausted) 
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

            character.ConsumeStamina(kickStaminaCost);
        }        
    }

    private void Awake()
    {
        character = GetComponent<PlayerCharacter>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(kickOrigin.position, kickRadius);
    }
}
