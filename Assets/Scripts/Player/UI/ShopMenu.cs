using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] GameObject shopMenu;
    [SerializeField] GameObject hud;

    public void ShopInteraction()
    {
        if (GameManager.Instance.playerCharacter.shopAvailable)
        {
            switch (shopMenu.activeSelf)
            {
                case true:
                    CloseShop();
                    break;

                case false:
                    OpenShop();
                    break;
            }
        }            
    }

    public bool IsShopActive()
    {
        if (shopMenu.activeSelf == true)
        {
            return true;
        }

        return false;
    }

    void OpenShop()
    {
        shopMenu.SetActive(true);
        //hud.SetActive(false);
        GameManager.Instance.playerHUD.SetActiveHUD(false);
        GameManager.Instance.playerInput.LockMouseInput(true);
        GameManager.Instance.playerInput.lockedInput = true;
    }

    void CloseShop()
    {
        shopMenu.SetActive(false);
        //hud.SetActive(true);
        GameManager.Instance.playerHUD.SetActiveHUD(true);
        GameManager.Instance.playerInput.LockMouseInput(false);
        GameManager.Instance.playerInput.lockedInput = false;
    }

    void Awake()
    {
        shopMenu.SetActive(false);
    }
}
