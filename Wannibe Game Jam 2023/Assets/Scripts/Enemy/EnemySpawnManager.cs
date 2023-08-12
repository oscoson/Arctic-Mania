using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] List<WaveInfo> waveInfo;
    [SerializeField] private TextMeshProUGUI waveCounterText; 
    int numberOfWaves = 0;
    int totalWaveEnemies = 0;
    int waveCounter = 0; 
    CombatManager combatManager;

    int currentEnemySpawnIndex = 0;  // the index for enemySpawnInfos

    List<EnemySpawner> enemySpawners;

    float delayTimer = 0.0f;

    void Start()
    {
        enemySpawners = new(Object.FindObjectsOfType<EnemySpawner>());
        numberOfWaves = waveInfo.Count;
        combatManager = FindObjectOfType<CombatManager>();
        totalWaveEnemies = GetNumberOfMobs();

        UpdateWaveCounter(); // Initial update
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;
        CheckPossibleSpawn();
    }

    private void FixedUpdate()
    {
        if (combatManager.mobCount == totalWaveEnemies)
        {
            Mob[] mobs = FindObjectsOfType<Mob>();
            foreach (Mob mob in mobs) {
                IFreezable freezableObject = mob as IFreezable;
                if (freezableObject == null) continue;

                if (!freezableObject.IsFrozen()) return;
            }

            // go to next wave
            waveCounter++;
            UpdateWaveCounter();
            currentEnemySpawnIndex = 0;
            combatManager.mobCount = 0;

            if (waveCounter >= numberOfWaves)
            {
                // should go to boss scene
                SceneManager.LoadScene("MainMenu");
                waveCounter = numberOfWaves - 1;
            }

            totalWaveEnemies = GetNumberOfMobs();

            foreach (Mob mob in mobs)
            {
                Destroy(mob.gameObject);
            }
        }
    }

    private int GetNumberOfMobs()
    {
        if (waveCounter >= numberOfWaves) return 0;

        int total = 0;
        foreach (var enemySpawnInfo in waveInfo[waveCounter].enemySpawnInfos)
        {
            total += enemySpawnInfo.basicMobCount;
            total += enemySpawnInfo.fireElementalMobCount;
            total += enemySpawnInfo.snowHareMobCount;
            total += enemySpawnInfo.arcticSealMobCount;
            total += enemySpawnInfo.huskyMobCount;
            total += enemySpawnInfo.foxMobCount;
        }
        return total;
    }

    private void CheckPossibleSpawn()
    {
        if (currentEnemySpawnIndex < waveInfo[waveCounter].enemySpawnInfos.Count)
        {
            EnemySpawnInfo enemySpawnInfo = waveInfo[waveCounter].enemySpawnInfos[currentEnemySpawnIndex];

            if (delayTimer >= enemySpawnInfo.delayFromLast)
            {
                int randomSpawnerIndex = Random.Range(0, enemySpawners.Count);

                /* try to find available spawner */
                for (int tryCount = 0; tryCount < enemySpawners.Count; tryCount++)
                {
                    if (!enemySpawners[randomSpawnerIndex].Busy) break;
                    randomSpawnerIndex = Random.Range(0, enemySpawners.Count);
                }

                enemySpawners[randomSpawnerIndex].AddToSpawnQueue(enemySpawnInfo);
                currentEnemySpawnIndex++;
                delayTimer = 0.0f;
            }
        }
    }

    private void UpdateWaveCounter()
    {
        waveCounterText.text = "Wave Number: " + (waveCounter + 1).ToString() + "/" + numberOfWaves.ToString();
    }
}

[System.Serializable]
public struct EnemySpawnInfo
{
    [SerializeField, Range(0.0f, 10.0f)] public float delayFromLast;  // how long (in seconds) to wait before the enemy spawns (since last spawn)
    [SerializeField, Range(0, 100)] public int basicMobCount;    // how many basic mobs to spawn
    [SerializeField, Range(0, 100)] public int fireElementalMobCount;    // how many fire elemental mobs to spawn
    [SerializeField, Range(0, 100)] public int snowHareMobCount;    // how many snow hare mobs to spawn
    [SerializeField, Range(0, 100)] public int arcticSealMobCount;    // how many arctic seal mobs to spawn
    [SerializeField, Range(0, 100)] public int huskyMobCount;    // how many husky mobs to spawn
    [SerializeField, Range(0, 100)] public int foxMobCount;    // how many fox mobs to spawn
}

[System.Serializable]
public struct WaveInfo
{
    public List<EnemySpawnInfo> enemySpawnInfos;
}
