using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionDamage;
    [SerializeField] float explosionForce;
    [SerializeField] float targetSize;
    [SerializeField] float sizeIncreaseFactor;
    [SerializeField] LayerMask entityLayer;

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
            Debug.Log("y esto tambien");
            other.SendMessage("TakeDamage", explosionDamage);
            //other.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, currentSize); TakeExplosionKnockback
        }       
    }
}
