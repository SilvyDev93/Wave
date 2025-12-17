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
    public bool gamePaused;

    public WaveManager waveManager;
    public AudioManager audioManager;
    public DebugManager debugManager;
    public PlayerManager playerManager;
    public ShopManager shopManager;

    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerCharacter playerCharacter;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public PlayerAbilities playerAbilities;    
    [HideInInspector] public PlayerHUD playerHUD;

    [HideInInspector] public WeaponHandler weaponHandler;

    [HideInInspector] public PauseMenu pauseMenu;
    [HideInInspector] public ShopMenu shopMenu;       
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

    void GetReferences()
    {
        Transform playerTransform = GameObject.Find("Player").transform;
        playerCharacter = playerTransform.GetChild(0).GetComponent<PlayerCharacter>();
        playerController = playerTransform.GetChild(0).GetComponent<PlayerController>();
        playerAbilities = playerTransform.GetChild(0).GetComponent<PlayerAbilities>();
        playerHUD = playerTransform.GetChild(1).GetComponent<PlayerHUD>();
        playerInput = playerTransform.gameObject.GetComponent<PlayerInput>();

        weaponHandler = GameObject.Find("WeaponHandler").GetComponent<WeaponHandler>();

        pauseMenu = playerTransform.GetChild(1).GetComponent<PauseMenu>();
        shopMenu = playerTransform.GetChild(1).GetComponent<ShopMenu>();        
        crosshairHandler = playerTransform.GetChild(1).GetComponent<CrosshairHandler>();

        audioManager.GetReferences();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ManagerInstancing();
        GetReferences();
       
        waveManager.StartWaving();
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        ManagerInstancing();
        GetReferences();
        DontDestroyOnLoad(gameObject);        
    }
}
