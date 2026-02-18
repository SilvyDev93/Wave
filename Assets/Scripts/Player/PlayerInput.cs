using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Android.AndroidGame;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    public bool lockedInput;
    public bool lockedMovement;
    public bool lockedMouse;

    int currentWeaponIndex = 0;

    void PlayerInputFixedUpdate()
    {
        if (!GameManager.Instance.gamePaused && !lockedInput)
        {
            MoveInput();
        }
    }

    void PlayerInputUpdate()
    {
        if (!GameManager.Instance.gamePaused && !lockedInput)
        {
            JumpInput();

            DashInput();
            
            FireInput();

            ReloadInput();

            WeaponChangeInput();

            KickInput();

            StompInput();
        }

        ShopInput();

        EscapeInput();
    }   

    void MoveInput()
    {
        if (!lockedMovement) 
        {
            controller.MovePlayer();
        }       
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
            GameManager.Instance.playerAbilities.Dash();
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
            WeaponHandler weaponHandler = GameManager.Instance.weaponHandler;
            weaponHandler.currentWeapon.PlayerReload();
            weaponHandler.DisplayAmmo();
        }
    }

    void WeaponChangeInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponIndex = 0;
            WeaponChange(currentWeaponIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponIndex = 1;
            WeaponChange(currentWeaponIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {            
            currentWeaponIndex = 2;
            WeaponChange(currentWeaponIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeaponIndex = 3;
            WeaponChange(currentWeaponIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentWeaponIndex = 4;
            WeaponChange(currentWeaponIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentWeaponIndex = 5;
            WeaponChange(currentWeaponIndex);
        }

        switch (Input.GetAxis("Mouse ScrollWheel"))
        {
            case > 0:
                currentWeaponIndex++;
                ValueScroll();
                WeaponChange(currentWeaponIndex);
                break;

            case < 0:
                currentWeaponIndex--;
                ValueScroll();
                WeaponChange(currentWeaponIndex);
                break;            
        }

        void ValueScroll()
        {
            if (currentWeaponIndex >= GameManager.Instance.weaponHandler.transform.childCount)
            {
                currentWeaponIndex = 0;
            }

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = GameManager.Instance.weaponHandler.transform.childCount - 1;
            }
        }

        void WeaponChange(int weaponIndex)
        {
            WeaponHandler weaponHandler = GameManager.Instance.weaponHandler;
            weaponHandler.ChangeWeapon(weaponIndex);
            StartCoroutine(weaponHandler.DisplayAmmo());
        }
    }

    void KickInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.playerAbilities.Kick();
        }
    }

    void StompInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameManager.Instance.playerAbilities.Stomp();
        }
    }

    void EscapeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (GameManager.Instance.shopMenu.IsShopActive())
            {
                case true:
                    GameManager.Instance.shopMenu.ShopInteraction();
                    break;

                case false:
                    GameManager.Instance.pauseMenu.PauseMenuHandler();
                    break;
            }            
        }
    }    

    public Vector3 GetHorizontalAxis()
    {
        if (!GameManager.Instance.gamePaused)
        return controller.characterDirection.forward * Input.GetAxis("Vertical");
        else return Vector3.zero;
    }

    public Vector3 GetVerticalAxis()
    {
        if (!GameManager.Instance.gamePaused)
        return controller.characterDirection.right * Input.GetAxis("Horizontal");
        else return Vector3.zero;
    }

    public Vector3 GetHorizontalInput()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            return Vector3.zero;
        }

        if (Input.GetKey(KeyCode.A))
        {
            return -GameManager.Instance.playerController.characterDirection.transform.right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            return GameManager.Instance.playerController.characterDirection.transform.right;
        }

        return Vector3.zero;
    }

    public Vector3 GetVerticalInput()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            return Vector3.zero;
        }

        if (Input.GetKey(KeyCode.W))
        {
            return GameManager.Instance.playerController.characterDirection.transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            return -GameManager.Instance.playerController.characterDirection.transform.forward;
        }

        return Vector3.zero;
    }

    public void SetAllPlayerInput(bool active)
    {
        lockedInput = active;
        lockedMouse = active;
    }

    void Update()
    {
        PlayerInputUpdate();
    }

    private void FixedUpdate()
    {       
        PlayerInputFixedUpdate();
    }
}
