using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    [Header("Weapon Listing")]

    [Header("Text")]
    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] TextMeshProUGUI weaponAmmo;
    [SerializeField] TextMeshProUGUI weaponWeight;
    [SerializeField] TextMeshProUGUI weaponSellValue;

    [Header("Objects")]
    [SerializeField] GameObject sellButton;
    [SerializeField] GameObject fillButton;
    [SerializeField] GameObject magButton;

    [Header("Images")]
    [SerializeField] RawImage weaponIcon;

    GameObject weaponObject;
    Weapon weapon;

    public void DisplayWeaponInfo(Weapon givenWeapon)
    {
        weapon = givenWeapon;
        weaponObject = givenWeapon.gameObject;

        weaponName.text = weapon.name;
        weaponAmmo.text = weapon.GetAmmoString();
        weaponWeight.text = weapon.weight.ToString();
        weaponSellValue.text = weapon.sellValue.ToString() + " $";

        weaponIcon.texture = weapon.weaponPortrait;

        if (!weapon.canBeSold)
        {
            sellButton.SetActive(false);    
        }

        if (!weapon.canBeRefilled)
        {
            fillButton.SetActive(false);
            magButton.SetActive(false);
        }
    }

    public void SellWeapon()
    {
        Debug.Log("Sold " + weapon.name);
        GameManager.Instance.playerManager.ManagePlayerMoney(weapon.sellValue);
        GameManager.Instance.RemovePlayerWeapon(weaponObject);
        transform.parent.parent.GetComponent<ShopHandling>().ShopRefresh();
    }
}
