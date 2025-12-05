using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Parameters")]
    [SerializeField] int waveCount;
    [SerializeField] int initialWaveTimer;
    [SerializeField] int waveTimer;
    [SerializeField] int enemyCooldown;

    [Header("Reward")]
    [SerializeField] int baseReward;
    [SerializeField] float rewardIncreaseFactor;

    [Header("References")]
    [SerializeField] Transform spawns;
    [SerializeField] GameObject enemy;
    
    int wave; int enemiesToSpawn; int previousReward; int[] waveList; bool gameStarted; bool startNextWave; float timer;

    PlayerHUD hud; Transform enemyParent;

    void SpawnEnemies()
    {
        if (enemiesToSpawn > 0)
        {
            Instantiate(enemy, ChooseSpawn().position, Quaternion.identity, enemyParent);
            enemiesToSpawn--;            
            StartCoroutine(SpawnCooldown());
        }             
    }

    IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(enemyCooldown);
        SpawnEnemies();
    }

    void NextWave()
    {        
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

        timer = initialWaveTimer;
        startNextWave = true;        
    }

    public void DestroyAllEntities()
    {
        if (enemyParent != null)
        {
            foreach (Transform child in enemyParent)
            {
                Destroy(child.gameObject);
            }
        }       
    }

    public void StartWaving()
    {
        DestroyAllEntities();
        GetReferences();
        WavesSetUp();
        previousReward = baseReward;
    }

    Transform ChooseSpawn()
    {
        Transform validSpawns = spawns.GetChild(0);
        return validSpawns.GetChild(Random.Range(0, validSpawns.childCount));
    }

    void Update()
    {
        if (enemiesToSpawn == 0 && enemyParent.childCount == 0 && gameStarted)
        {
            gameStarted = false;
            WaveReward();
            GameManager.Instance.shopManager.SpawnShop();
            timer = waveTimer;
            startNextWave = true;
        }

        if (startNextWave)
        {
            if (timer < 0)
            {
                startNextWave = false;
                GameManager.Instance.shopManager.DestroyShop();
                NextWave();
            }
            else
            {
                timer = timer - 1 * Time.deltaTime;
                hud.enemyCounter.text = ((int)timer).ToString();
            }            
        }
    }   
}
