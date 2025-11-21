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

    [HideInInspector] public WaveManager waveManager;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public PlayerCharacter playerCharacter;
    [HideInInspector] public PlayerHUD playerHUD;
    [HideInInspector] public ShopMenu shopMenu;
    [HideInInspector] public PauseMenu pauseMenu;
    [HideInInspector] public DebugManager debugManager;
    [HideInInspector] public CrosshairHandler crosshairHandler;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public WeaponHandler weaponHandler;
    [HideInInspector] public PlayerAbilities playerAbilities;
    [HideInInspector] public PlayerInput playerInput;

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
        waveManager = transform.GetChild(0).GetComponent<WaveManager>();
        audioManager = transform.GetChild(1).GetComponent<AudioManager>();
        debugManager = transform.GetChild(3).GetComponent<DebugManager>();

        Transform playerTransform = GameObject.Find("Player").transform;
        playerCharacter = playerTransform.GetChild(0).GetComponent<PlayerCharacter>();
        playerController = playerTransform.GetChild(0).GetComponent<PlayerController>();
        playerAbilities = playerTransform.GetChild(0).GetComponent<PlayerAbilities>();
        playerHUD = playerTransform.GetChild(1).GetComponent<PlayerHUD>();
        shopMenu = playerTransform.GetChild(1).GetComponent<ShopMenu>();
        pauseMenu = playerTransform.GetChild(1).GetComponent<PauseMenu>();
        crosshairHandler = playerTransform.GetChild(1).GetComponent<CrosshairHandler>();
        weaponHandler = GameObject.Find("WeaponHandler").GetComponent<WeaponHandler>();
        playerInput = playerTransform.gameObject.GetComponent<PlayerInput>();
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
        
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        ManagerInstancing();
        GetReferences();
        DontDestroyOnLoad(gameObject);        
    }
}
