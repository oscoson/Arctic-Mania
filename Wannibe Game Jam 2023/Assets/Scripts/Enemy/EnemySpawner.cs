using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Mob> enemies;                   // only used to retrieve enemyPrefabs
    public Queue<EnemySpawnInfo> spawnQueue;

    Dictionary<EnemyID, Mob> enemyPrefabs;      // maps enemyId to enemyPrefabs

    public float cooldown = 3.0f;                   // cooldown makes it so the spawner doesn't spawn too many at once
    private float cooldownTimer = float.MaxValue;   // gets reset to 0 when timer is reset
    private float startVisualizerTimer;
    private Animator spawnIndication;
    
    public bool Busy { get; private set; }

    private CombatManager combatManager;

    void Start()
    {
        /* init */
        combatManager = FindObjectOfType<CombatManager>();
        spawnIndication = GetComponent<Animator>();
        spawnQueue = new();
        enemyPrefabs = new();
        
        // load mobs to dict
        for (int i = 0; i < enemies.Count; i++)
        {
            Mob mob = enemies[i];
            if (mob is BasicMob)
            {
                enemyPrefabs.Add(EnemyID.BasicMob, enemies[i]);
            }
            else if (mob is FireElementalMob)
            {
                enemyPrefabs.Add(EnemyID.FireElementalMob, enemies[i]);
            }
            else if (mob is SnowHareMob)
            {
                enemyPrefabs.Add(EnemyID.SnowHareMob, enemies[i]);
            }
            else if (mob is ArcticSealMob)
            {
                enemyPrefabs.Add(EnemyID.ArcticSealMob, enemies[i]);
            }
            else if (mob is HuskyMob)
            {
                enemyPrefabs.Add(EnemyID.HuskyMob, enemies[i]);
            }
            else if (mob is FoxMob)
            {
                enemyPrefabs.Add(EnemyID.FoxMob, enemies[i]);
            }
        }
    }

    void Update()
    {
        /* cooldown logic */
        cooldownTimer += Time.deltaTime;
        Busy = cooldownTimer < cooldown;
        cooldownTimer = Mathf.Min(cooldownTimer, cooldown);

        if (!Busy && spawnQueue.Count > 0) 
        {
            startVisualizerTimer += Time.deltaTime;
            if(startVisualizerTimer > 3)
            {
                startVisualizerTimer = 0;
                SpawnEnemy(spawnQueue.Dequeue());  // spawn if not busy
                spawnIndication.enabled = false;
            }
            else
            {
                spawnIndication.enabled = true;
            }

        }
    }

    public void AddToSpawnQueue(EnemySpawnInfo enemySpawnInfo)
    {
        spawnQueue.Enqueue(enemySpawnInfo);
    }

    private void SpawnEnemy(EnemySpawnInfo enemySpawnInfo)
    {
        float spawnRadius = 2.0f;
        for (int i = 0; i < enemySpawnInfo.basicMobCount; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefabs[EnemyID.BasicMob].gameObject, randomPosition, Quaternion.identity);
            combatManager.mobCount++;
        }

        for (int i = 0; i < enemySpawnInfo.fireElementalMobCount; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefabs[EnemyID.FireElementalMob].gameObject, randomPosition, Quaternion.identity);
            combatManager.mobCount++;
        }

        for (int i = 0; i < enemySpawnInfo.snowHareMobCount; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefabs[EnemyID.SnowHareMob].gameObject, randomPosition, Quaternion.identity);
            combatManager.mobCount++;
        }

        for (int i = 0; i < enemySpawnInfo.arcticSealMobCount; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefabs[EnemyID.ArcticSealMob].gameObject, randomPosition, Quaternion.identity);
            combatManager.mobCount++;
        }

        for (int i = 0; i < enemySpawnInfo.huskyMobCount; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefabs[EnemyID.HuskyMob].gameObject, randomPosition, Quaternion.identity);
            combatManager.mobCount++;
        }

        for (int i = 0; i < enemySpawnInfo.foxMobCount; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefabs[EnemyID.FoxMob].gameObject, randomPosition, Quaternion.identity);
            combatManager.mobCount++;
        }
        cooldownTimer = 0.0f;
    }

    
}
