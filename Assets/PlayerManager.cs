using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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

    private void Start()
    {
        playerCharacter = GameManager.Instance.playerCharacter;
    }
}
