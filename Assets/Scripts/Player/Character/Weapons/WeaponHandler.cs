using System.Collections;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class WeaponHandler : MonoBehaviour
{
    [HideInInspector] public GameObject[] playerWeapons; [HideInInspector] public WeaponSlot currentSlot; [HideInInspector] public Weapon currentWeapon; [HideInInspector] public bool slotsReady;

    public void ChangeWeapon(int weaponID)
    {
        if (!currentWeapon.reloading)
        {
            if (weaponID <= playerWeapons.Length)
            {
                StartCoroutine(ChangeCoroutine());
            }
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
            StartCoroutine(DisplayAmmo());
        }
    }

    public Weapon[] GetAllPlayerWeapons()
    {
        Weapon[] weapons = new Weapon[playerWeapons.Length];

        for (int i = 0; i < playerWeapons.Length; i++)
        {
            weapons[i] = playerWeapons[i].GetComponent<Weapon>();
        }

        return weapons;
    }
    
    IEnumerator GetWeapons()
    {
        playerWeapons = new GameObject[transform.childCount];

        for (int i = 0; i < playerWeapons.Length; i++)
        {
            if (transform.GetChild(i) != null)
            {
                playerWeapons[i] = transform.GetChild(i).gameObject;
            }            
        }

        currentWeapon = playerWeapons[0].GetComponent<Weapon>();

        yield return new WaitUntil(() => slotsReady);

        currentSlot = GameManager.Instance.playerHUD.GetWeaponSlot(0);
        currentSlot.Selection();

        foreach (GameObject weapon in playerWeapons)
        {
            weapon.SetActive(false);
        }

        playerWeapons[0].SetActive(true);
    }

    public IEnumerator DisplayAmmo()
    {
        yield return new WaitUntil(() => slotsReady);
        
        Transform weaponSlots = GameManager.Instance.playerHUD.weaponSlots;

        for (int i = 0; i < playerWeapons.Length; i++)
        {
            weaponSlots.GetChild(i).GetComponent<WeaponSlot>().SetAmmoString(playerWeapons[i].GetComponent<Weapon>().GetAmmoString());
        }
    }

    public void DisplayAmmo2()
    {
        Transform weaponSlots = GameManager.Instance.playerHUD.weaponSlots;

        for (int i = 0; i < playerWeapons.Length; i++)
        {
            weaponSlots.GetChild(i).GetComponent<WeaponSlot>().SetAmmoString(playerWeapons[i].GetComponent<Weapon>().GetAmmoString());
        }
    }

    void Start()
    {
        StartCoroutine(GetWeapons());       
    }
}
