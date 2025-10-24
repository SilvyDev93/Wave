using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static Instance
    public static GameManager Instance;
    private GameManager() { }

    [HideInInspector] public WaveManager waveManager;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public PlayerCharacter playerCharacter;
    [HideInInspector] public PlayerHUD playerHUD;

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

    void GetReferences()
    {
        waveManager = transform.GetChild(0).GetComponent<WaveManager>();
        audioManager = transform.GetChild(1).GetComponent<AudioManager>();
        
        Transform playerTransform = GameObject.Find("Player").transform;
        playerCharacter = playerTransform.GetChild(0).GetComponent<PlayerCharacter>();
        playerHUD = playerTransform.GetChild(1).GetComponent<PlayerHUD>();
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
