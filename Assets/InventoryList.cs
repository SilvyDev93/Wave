using UnityEngine;

public class InventoryList : MonoBehaviour
{
    public Weapon[] weapons;

    public void GiveChildrenWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (transform.GetChild(i) != null)
            {
                transform.GetChild(i).gameObject.GetComponent<WeaponInventory>().DisplayWeaponInfo(weapons[i]);
            }
        }
    }
}
