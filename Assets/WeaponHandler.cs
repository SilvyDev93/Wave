using System.Collections;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class WeaponHandler : MonoBehaviour
{
    GameObject[] playerWeapons; [HideInInspector] public Weapon currentWeapon;

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
    }

    void Start()
    {
        GetWeapons();
    }
}
