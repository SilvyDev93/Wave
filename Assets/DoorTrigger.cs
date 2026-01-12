using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    DoorBehavior doorBehavior;

    void OnTriggerEnter(Collider other)
    {
        doorBehavior.DoorActivation(true);
    }

    void OnTriggerExit(Collider other)
    {
        doorBehavior.DoorActivation(false);
    }

    void Awake()
    {
        doorBehavior = transform.parent.GetComponent<DoorBehavior>();
    }
}
