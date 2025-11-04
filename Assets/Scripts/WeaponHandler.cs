using System.Collections;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class WeaponHandler : MonoBehaviour
{
    GameObject[] playerWeapons; [HideInInspector] public WeaponSlot currentSlot; [HideInInspector] public Weapon currentWeapon;

    public void ChangeWeapon(int weaponID)
    {
        if (weaponID <= playerWeapons.Length)
        {
            StartCoroutine(ChangeCoroutine());
        }
        
        IEnumerator ChangeCoroutine()
        {
            yield return new WaitUntil(() => currentWeapon.shooting == false);

            foreach (GameObject weapon in playerWeapons)
            {
                weapon.SetActive(false);
            }

            playerWeapons[weaponID].SetActive(true);
            currentWeapon = playerWeapons[weaponID].GetComponent<Weapon>();

            if (currentSlot != null)
            {
                currentSlot.Selection();
            }

            currentSlot = GameManager.Instance.playerHUD.GetWeaponSlot(weaponID);
            currentSlot.Selection();
        }
    }
    
    void GetWeapons()
    {
        playerWeapons = new GameObject[3];

        for (int i = 0; i < playerWeapons.Length; i++)
        {
            if (transform.GetChild(i) != null)
            {
                playerWeapons[i] = transform.GetChild(i).gameObject;
            }            
        }

        currentWeapon = playerWeapons[0].GetComponent<Weapon>();
        currentSlot = GameManager.Instance.playerHUD.GetWeaponSlot(0);
        currentSlot.Selection();
    }

    public void DisplayAmmo()
    {
        Transform weaponSlots = GameManager.Instance.playerHUD.weaponSlots;

        weaponSlots.GetChild(0).GetComponent<WeaponSlot>().SetAmmoString(playerWeapons[0].GetComponent<Weapon>().GetAmmoString());
        weaponSlots.GetChild(1).GetComponent<WeaponSlot>().SetAmmoString(playerWeapons[1].GetComponent<Weapon>().GetAmmoString());
        weaponSlots.GetChild(2).GetComponent<WeaponSlot>().SetAmmoString(playerWeapons[2].GetComponent<Weapon>().GetAmmoString());
    }

    void Update()
    {
        DisplayAmmo();
    }

    void Start()
    {
        GetWeapons();       
    }
}
