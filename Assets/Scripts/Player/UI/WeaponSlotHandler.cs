using System.Collections;
using UnityEngine;

public class WeaponSlotHandler : MonoBehaviour
{
    [SerializeField] GameObject weaponSlotPrefab;
    void WeaponSlotsCreation()
    {
        WeaponHandler weaponHandler = GameManager.Instance.weaponHandler;

        for (int i = 0; i < weaponHandler.playerWeapons.Length; i++)
        {
            GameObject slot = Instantiate(weaponSlotPrefab, transform);
            slot.name = "Slot " + i;
            slot.GetComponent<WeaponSlot>().SetWeaponPortrait(weaponHandler.GetWeapon(i).weaponPortrait);
        }

        weaponHandler.slotsReady = true;
    }

    void Start()
    {
        WeaponSlotsCreation();
        StartCoroutine(GameManager.Instance.weaponHandler.DisplayAmmo());
    }
}
