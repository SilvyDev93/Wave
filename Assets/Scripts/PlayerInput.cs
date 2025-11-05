using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Android.AndroidGame;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    public bool lockedInput;

    void PlayerInputUpdate()
    {
        if (!GameManager.Instance.gamePaused && !lockedInput)
        {
            JumpInput();

            DashInput();

            ShopInput();

            FireInput();

            ReloadInput();

            WeaponChangeInput();

            KickInput();
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
        Weapon currentWeapon = GameManager.Instance.weaponHandler.currentWeapon;

        switch (currentWeapon.fireMode.ToString())
        {
            case "Automatic":

                if (Input.GetMouseButton(0))
                {
                    currentWeapon.PlayerTriggerPush();
                }

                break;

            default:

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
            GameManager.Instance.weaponHandler.currentWeapon.PlayerReload();
        }
    }

    void WeaponChangeInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.weaponHandler.ChangeWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.weaponHandler.ChangeWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.Instance.weaponHandler.ChangeWeapon(2);
        }
    }

    void KickInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.playerAbilities.Kick();
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
