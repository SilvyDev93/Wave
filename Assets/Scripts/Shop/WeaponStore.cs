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

    GameObject weaponObject;
    Weapon weapon;

    public void DisplayWeaponInfo(Object givenWeapon)
    {
        weaponObject = (GameObject)givenWeapon;
        weapon = weaponObject.GetComponent<Weapon>();

        weaponName.text = weapon.name;
        weaponPrice.text = weapon.price.ToString() + " $";
        weaponWeight.text = weapon.weight.ToString();
        weaponIcon.texture = weapon.weaponPortrait;
    }

    public void BuyWeapon()
    {
        if (GameManager.Instance.playerManager.GetPlayerMoney() >= weapon.price)
        {
            if (!GameManager.Instance.playerManager.HasWeaponID(weapon.id))
            {
                Debug.Log("Bought " + weapon.name);
                GameManager.Instance.playerManager.ManagePlayerMoney(-weapon.price);
                GameManager.Instance.GivePlayerWeapon(weaponObject);
                StartCoroutine(transform.parent.parent.GetComponent<ShopHandling>().TempUpdateUse(0.5f));
                //StartCoroutine(GameManager.Instance.weaponHandler.DisplayAmmo());
                //transform.parent.parent.GetComponent<ShopHandling>().ShopRefresh();
            }
            else
            {
                Debug.Log("Player already has this weapon");
            }
        }
        else
        {
            Debug.Log("Not enought money to buy: " + weapon.name);
        }
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }
}
