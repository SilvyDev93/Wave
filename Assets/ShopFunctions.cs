using UnityEngine;

public class ShopFunctions : MonoBehaviour
{
    WeaponHandler weaponHandler;

    public void CloseShopMenu()
    {
        GameManager.Instance.shopMenu.ShopInteraction();
    }

    public void GetPlayerWeapon()
    {

    }

    private void Start()
    {
        weaponHandler = GameManager.Instance.weaponHandler;
    }
}
