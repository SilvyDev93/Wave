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

    [Header("Images")]
    [SerializeField] RawImage weaponIcon;

    Weapon weapon;

    public void DisplayWeaponInfo(Weapon givenWeapon)
    {
        weapon = givenWeapon;

        weaponName.text = weapon.name;
        weaponAmmo.text = weapon.GetAmmoString();
        weaponWeight.text = weapon.weight.ToString();
        weaponSellValue.text = weapon.sellValue.ToString();
    }
}
