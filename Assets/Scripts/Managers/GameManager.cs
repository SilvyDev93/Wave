using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static Instance
    public static GameManager Instance;
    private GameManager() { }

    [Header("Global Parameters")]
    public float globalGravity = -9.81f;
    public int frameRate;
    public bool gamePaused;

    [Header("Managers")]
    public WaveManager waveManager;
    public AudioManager audioManager;
    public DebugManager debugManager;
    public PlayerManager playerManager;
    public ShopManager shopManager;
    public VolumeManager volumeManager;
    
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerCharacter playerCharacter;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public PlayerAbilities playerAbilities;    
    [HideInInspector] public PlayerHUD playerHUD;

    [HideInInspector] public WeaponHandler weaponHandler;
    [HideInInspector] public WeaponSlotHandler weaponSlotHandler;

    [HideInInspector] public PauseMenu pauseMenu;
    [HideInInspector] public ShopMenu shopMenu;
    public ShopHandling shopHandling;
    [HideInInspector] public CrosshairHandler crosshairHandler;
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        MouseLockedState(false);
        gamePaused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        MouseLockedState(true);
        gamePaused = false;
    }

    public void MouseLockedState(bool state)
    {
        switch (state)
        {
            case true:
                LockMouse();
                break;

            case false:
                UnlockMouse();
                break;
        }

        void LockMouse()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void UnlockMouse()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void ManagerInstancing()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public bool ProbabilityCalculation(float chance)
    {
        if (chance >= UnityEngine.Random.Range(1, 101))
        {
            return true;
        }

        return false;
    }
    
    public void GivePlayerWeapon(GameObject weapon)
    {
        GameObject newWeapon = Instantiate(weapon, weaponHandler.transform);
        newWeapon.name = weapon.name;
        newWeapon.SetActive(false);
        StartCoroutine(weaponHandler.GetWeapons());
        weaponHandler.ChangeWeapon(weaponHandler.playerWeapons.Length - 1);
        weaponSlotHandler.WeaponSlotsReset();
        playerManager.AddWeaponID(weapon.GetComponent<Weapon>().id);
        shopHandling.ShopRefresh();
    }

    public void RemovePlayerWeapon(GameObject weapon)
    {
        if (weaponHandler.playerWeapons.Length > 1)
        {
            GameObject weaponObject = ReturnPlayerPossesionOfWeapon(weapon);

            if (weaponObject != null)
            {
                playerManager.RemoveWeaponID(weapon.GetComponent<Weapon>().id);
                DestroyImmediate(weaponObject);
                StartCoroutine(weaponHandler.GetWeapons());                
                weaponSlotHandler.WeaponSlotsReset();
            }
        }
        else
        {
            Debug.Log("Cant sell with just one weapon");
        }
    }

    public GameObject ReturnPlayerPossesionOfWeapon(GameObject weaponObject)
    {
        foreach (Transform t in weaponHandler.transform)
        {
            if (t.gameObject == weaponObject)
            {
                return t.gameObject;
            }
        }

        return null;
    }

    void GetReferences()
    {
        if (InGameplayScene())
        {
            Transform playerTransform = GameObject.Find("Player").transform;

            if (playerTransform != null)
            {
                playerCharacter = playerTransform.GetChild(0).GetComponent<PlayerCharacter>();
                playerController = playerTransform.GetChild(0).GetComponent<PlayerController>();
                playerAbilities = playerTransform.GetChild(0).GetComponent<PlayerAbilities>();
                playerHUD = playerTransform.GetChild(1).GetComponent<PlayerHUD>();
                playerInput = playerTransform.gameObject.GetComponent<PlayerInput>();
                pauseMenu = playerTransform.GetChild(1).GetComponent<PauseMenu>();
                shopMenu = playerTransform.GetChild(1).GetComponent<ShopMenu>();
                
                crosshairHandler = playerTransform.GetChild(1).GetComponent<CrosshairHandler>();
            }

            weaponHandler = GameObject.Find("WeaponHandler").GetComponent<WeaponHandler>();
            weaponSlotHandler = GameObject.Find("WeaponSlots").GetComponent<WeaponSlotHandler>();

            audioManager.GetReferences();
        }       
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void DestroyManager()
    {
        if (!InGameplayScene())
        {
            Destroy(gameObject);
        }
    }

    bool InGameplayScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DestroyManager();
        ManagerInstancing();        
        GetReferences();
        UnPauseGame();
        waveManager.StartWaving();
    }

    void Awake()
    {
        Application.targetFrameRate = frameRate;
        ManagerInstancing();
        GetReferences();
        DontDestroyOnLoad(gameObject);        
    }
}
