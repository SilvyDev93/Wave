using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform destination;

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = destination.position;
    }
}
