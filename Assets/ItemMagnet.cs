using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            other.gameObject.AddComponent<PlayerAttraction>();
        }
    }
}
