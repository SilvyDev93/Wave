using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] LayerMask entityLayer;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.SendMessage("KillEntity");
        }

        if (other.gameObject.layer == entityLayer)
        {
            other.SendMessage("KillEntity"); 
        }
    }
}
