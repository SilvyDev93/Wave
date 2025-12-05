using UnityEngine;

public class SpawnNearPlayerPrevention : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent = transform.parent.parent.GetChild(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent = transform.parent.parent.GetChild(0);
        }
    }
}
