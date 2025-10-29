using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Android.AndroidGame;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerController controller;

    void PlayerInputUpdate()
    {
        if (!GameManager.Instance.gamePaused)
        {
            JumpInput();

            DashInput();

            ShopInput();

            FireInput();

            ReloadInput();
        }

        PauseInput();
    }

    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.Jump();
        }
    }

    void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.Dash();
        }
    }

    void PauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.pauseMenu.PauseMenuHandler();
        }
    }

    void ShopInput()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameManager.Instance.shopMenu.ShopInteraction();
        }
    }

    void FireInput()
    {
        Weapon currentWeapon = GameManager.Instance.playerCharacter.currentWeapon;

        switch (currentWeapon.fireMode.ToString())
        {
            case "Automatic":

                if (Input.GetMouseButton(0))
                {
                    currentWeapon.PlayerTriggerPush();
                }

                break;

            case "Semiautomatic":

                if (Input.GetMouseButtonDown(0))
                {
                    currentWeapon.PlayerTriggerPush();
                }

                break;

            case "Manual":

                if (Input.GetMouseButtonDown(0))
                {
                    currentWeapon.PlayerTriggerPush();
                }

                break;
        }
    }
    
    void ReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.playerCharacter.currentWeapon.PlayerReload();
        }
    }

    public Vector3 GetHorizontalAxis()
    {
        if (!GameManager.Instance.gamePaused)
        return controller.transform.forward * Input.GetAxis("Vertical");
        else return Vector3.zero;
    }

    public Vector3 GetVerticalAxis()
    {
        if (!GameManager.Instance.gamePaused)
        return controller.transform.right * Input.GetAxis("Horizontal");
        else return Vector3.zero;
    }

    void Update()
    {
        PlayerInputUpdate();
    }
}
