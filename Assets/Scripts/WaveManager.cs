using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Parameters")]
    [SerializeField] int waveCount;
    [SerializeField] int waveTimer;
    [SerializeField] int enemyCooldown;

    [Header("Reward")]
    [SerializeField] int baseReward;
    [SerializeField] float rewardIncreaseFactor;

    [Header("References")]
    [SerializeField] GameObject enemy;
    
    int wave; int enemiesToSpawn; int previousReward; int[] waveList; bool gameStarted;

    PlayerHUD hud; Transform enemyParent;

    void SpawnEnemies()
    {
        if (enemiesToSpawn > 0)
        {
            Instantiate(enemy, transform.position, Quaternion.identity, enemyParent);
            Debug.Log("Enemy spawn");
            enemiesToSpawn--;            
            StartCoroutine(SpawnCooldown());
        }             
    }

    IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(enemyCooldown);
        SpawnEnemies();
    }

    IEnumerator NextWave()
    {        
        yield return new WaitForSeconds(waveTimer);
        wave++;
        hud.waveCounter.text = "Wave " + wave.ToString();
        enemiesToSpawn = waveList[wave];
        hud.enemyCounter.text = enemiesToSpawn.ToString();
        gameStarted = true;
        SpawnEnemies();
    }

    void WaveReward()
    {
        int reward = ((int)(previousReward + (previousReward * rewardIncreaseFactor)));
        GameManager.Instance.playerCharacter.GetMoney(reward);
        previousReward = reward;
    }

    void GetReferences()
    {
        hud = GameManager.Instance.playerHUD;
        enemyParent = transform.GetChild(0);
    }

    void WavesSetUp()
    {
        waveList = new int[waveCount];

        for (int i = 0; i < waveList.Length; i++)
        {
            waveList[i] = 10;
        }

        StartCoroutine(NextWave());
    }

    void Update()
    {
        if (enemiesToSpawn == 0 && enemyParent.childCount == 0 && gameStarted)
        {
            gameStarted = false;
            WaveReward();
            StartCoroutine(NextWave());
        }
    }

    void Start()
    {
        GetReferences();
        WavesSetUp();
        previousReward = baseReward;
    }   
}
