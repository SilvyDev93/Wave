using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] GameObject shopMenu;

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

    void OpenShop()
    {
        shopMenu.SetActive(true);
        GameManager.Instance.MouseLockedState(false);
    }

    void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.Instance.MouseLockedState(true);
    }

    void Awake()
    {
        shopMenu.SetActive(false);
    }
}
