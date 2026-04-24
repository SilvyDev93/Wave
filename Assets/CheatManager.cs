using System.Linq;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public void CheatPlayerAllWeapons()
    {
        GameObject[] weapons = Resources.LoadAll("Weapons").Cast<GameObject>().ToArray();

        for (int i = 0; i < weapons.Length; i++)
        {
            GameManager.Instance.GivePlayerWeapon(weapons[i]);
        }       
    }
}
