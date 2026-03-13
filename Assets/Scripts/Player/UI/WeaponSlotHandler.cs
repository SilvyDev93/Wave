using System.Collections;
using UnityEngine;

public class WeaponSlotHandler : MonoBehaviour
{
    [SerializeField] GameObject weaponSlotPrefab;
    public void WeaponSlotsCreation()
    {
        WeaponHandler weaponHandler = GameManager.Instance.weaponHandler;

        for (int i = 0; i < weaponHandler.playerWeapons.Length; i++)
        {
            GameObject slot = Instantiate(weaponSlotPrefab, transform);
            slot.name = "Slot " + i;
            slot.GetComponent<WeaponSlot>().SetWeaponPortrait(weaponHandler.GetWeapon(i).weaponPortrait);
            slot.GetComponent<WeaponSlot>().SetWeaponIndex(i + 1);
        }

        weaponHandler.slotsReady = true;

        bool originalActiveState = gameObject.activeSelf;

        gameObject.SetActive(true);
        StartCoroutine(GameManager.Instance.weaponHandler.DisplayAmmo());
        gameObject.SetActive(originalActiveState);
    }

    public void WeaponSlotsReset()
    {
        WeaponHandler weaponHandler = GameManager.Instance.weaponHandler;
        weaponHandler.slotsReady = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        WeaponSlotsCreation();
    }

    void Start()
    {
        WeaponSlotsCreation();      
    }
}
