using UnityEngine;

public class HealthShopRecovery : MonoBehaviour
{
    PlayerCharacter character;
    PlayerManager manager;

    public void BuyHealth()
    {
        character = GameManager.Instance.playerCharacter;
        manager = GameManager.Instance.playerManager;

        int healthPrice = (int)(character.health - character.currentHealth);
        Debug.Log(healthPrice);

        if (healthPrice > 0)
        {
            if (manager.GetPlayerMoney() >= healthPrice)
            {
                manager.ManagePlayerMoney(-healthPrice);
                character.RecoverHealth(healthPrice);
            }
            else
            {
                Debug.Log("Not enought money to buy health");
            }
        }       
    }
}
