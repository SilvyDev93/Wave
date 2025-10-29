using UnityEngine;

public class ShopArea : MonoBehaviour
{
    public void PlayerEnterShop()
    {
        GameManager.Instance.playerHUD.shopCaption.gameObject.SetActive(true);
        GameManager.Instance.playerCharacter.shopAvailable = true;
    }

    public void PlayerExitShop()
    {
        GameManager.Instance.playerHUD.shopCaption.gameObject.SetActive(false);
        GameManager.Instance.playerCharacter.shopAvailable = false;
    }
}
