using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] bool gizmosActive;

    ShopArea shopArea;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            shopArea.PlayerEnterShop();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            shopArea.PlayerExitShop();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (gizmosActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }       
    }

    void Awake()
    {
        shopArea = transform.parent.GetComponent<ShopArea>();
    }
}
