using UnityEngine;

public class ShopList : MonoBehaviour
{
    public Object[] weapons;

    public void GiveChildrenWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (transform.GetChild(i) != null)
            {
                transform.GetChild(i).gameObject.GetComponent<WeaponStore>().DisplayWeaponInfo(weapons[i]);
            }            
        }
    }
}
