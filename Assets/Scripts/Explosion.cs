using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionDamage;
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
        other.SendMessage("TakeDamage", explosionDamage);
    }
}
