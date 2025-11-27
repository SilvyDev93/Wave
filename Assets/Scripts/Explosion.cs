using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] float targetSize;
    [SerializeField] float sizeIncreaseFactor;
    [SerializeField] LayerMask entityLayer;

    [Header("Player Interaction")]
    [SerializeField] float playerDamage;
    [SerializeField] float playerForce;
    [SerializeField] float playerUpwardsForce;

    [Header("NPC Interaction")]
    [SerializeField] float npcDamage;
    [SerializeField] float npcForce;
    [SerializeField] float npcUpwardsForce;

    float currentSize;

    void SizeIncrease()
    {
        currentSize += sizeIncreaseFactor;
        currentSize = Mathf.Clamp(currentSize, 0, targetSize);
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);
    }

    void Update()
    {
        SizeIncrease();
    }

    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == entityLayer)
        {
            switch (other.tag)
            {
                case "Player":
                    other.SendMessage("TakeDamage", playerDamage);
                    other.GetComponent<Rigidbody>().AddExplosionForce(playerForce, transform.position, currentSize, playerUpwardsForce, ForceMode.Impulse);
                    break;

                case "Enemy":

                    if (!other.GetComponent<CharacterNPC>().onExplosionCooldown)
                    {
                        other.SendMessage("TakeDamage", npcDamage);
                        other.GetComponent<CharacterNPC>().SeparateFromGround();
                        other.GetComponent<Rigidbody>().AddExplosionForce(npcForce, transform.position, currentSize, npcUpwardsForce, ForceMode.Impulse);
                        //StartCoroutine(other.GetComponent<CharacterNPC>().ExplosionDamageCooldown());
                        other.GetComponent<CharacterNPC>().onExplosionCooldown = true;
                    }
                    
                    break;
            }  
        }       
    }
}
