using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool playerInmortal = false;
    List<int> weaponIdList = new List<int>();
    PlayerCharacter playerCharacter;

    public void ManagePlayerMoney(int amount)
    {
        playerCharacter.GetMoney(amount);
    }

    public int GetPlayerMoney()
    {
        if (playerCharacter != null)
        {
            return playerCharacter.money;
        }

        return 0;
    }

    public void AddWeaponID(int id)
    {
        weaponIdList.Add(id);
    }

    public void RemoveWeaponID(int id)
    {
        weaponIdList.Remove(id);
    }

    public bool HasWeaponID(int id)
    {
        if (weaponIdList.Contains(id))
        {
            return true;
        }
        else { return false; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            playerInmortal = true;
        }       
    }

    private void Start()
    {
        playerCharacter = GameManager.Instance.playerCharacter;
    }
}
