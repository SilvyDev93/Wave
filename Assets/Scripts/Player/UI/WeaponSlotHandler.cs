using System.Collections;
using UnityEngine;

public class WeaponSlotHandler : MonoBehaviour
{
    [SerializeField] GameObject weaponSlotPrefab;
    void WeaponSlotsCreation()
    {
        for (int i = 0; i < GameManager.Instance.weaponHandler.playerWeapons.Length; i++)
        {
            GameObject slot = Instantiate(weaponSlotPrefab, transform);
            slot.name = "Slot " + i;
        }

        GameManager.Instance.weaponHandler.slotsReady = true;
    }

    void Start()
    {
        WeaponSlotsCreation();
        StartCoroutine(GameManager.Instance.weaponHandler.DisplayAmmo());
    }
}
