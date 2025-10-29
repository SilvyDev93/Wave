using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
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
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    void Awake()
    {
        shopArea = transform.parent.GetComponent<ShopArea>();
    }
}
