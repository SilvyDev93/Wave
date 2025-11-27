using UnityEngine;

public class ShopFunctions : MonoBehaviour
{
    public void CloseShopMenu()
    {
        GameManager.Instance.shopMenu.ShopInteraction();
    }
}
