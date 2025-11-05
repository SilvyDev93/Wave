using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] LayerMask entityLayer;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == entityLayer)
        {
            other.SendMessage("KillEntity"); 
            Debug.Log("Entity Killed");
        }

        other.SendMessage("KillEntity");
        Debug.Log("Entity Killed");

        Debug.Log("Trigger Event");
    }
}
