using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform destination;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = destination.position;
        }        
    }
}
