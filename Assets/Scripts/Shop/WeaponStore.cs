using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WeaponStore : MonoBehaviour
{
    [Header("Weapon Listing")]

    [Header("Text")]
    [SerializeField] TextMeshProUGUI weaponPrice;
    [SerializeField] TextMeshProUGUI weaponWeight;
    [SerializeField] TextMeshProUGUI weaponName;

    [Header("Images")]
    [SerializeField] RawImage weaponIcon;
    [SerializeField] RawImage typeIcon;

    Weapon weapon;

    public void DisplayWeaponInfo(Object givenWeapon)
    {
        GameObject newWeapon = (GameObject)givenWeapon;
        weapon = newWeapon.GetComponent<Weapon>();

        weaponName.text = weapon.name;
        weaponPrice.text = weapon.price.ToString();
        weaponWeight.text = weapon.weight.ToString();
    }

    public void BuyWeapon()
    {
        if (GameManager.Instance.playerManager.GetPlayerMoney() >= weapon.price)
        {
            Debug.Log("Bought " + weapon.name);
            GameManager.Instance.playerManager.ManagePlayerMoney(-weapon.price);
            transform.parent.parent.GetComponent<ShopHandling>().ShopRefresh();
        }
        else
        {
            Debug.Log("Not enought money to buy: " + weapon.name);
        }
    }
}
