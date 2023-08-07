using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] List<EnemySpawnInfo> enemySpawnInfos;
    int currentEnemySpawnIndex = 0;  // the index for enemySpawnInfos

    [SerializeField] List<EnemySpawner> enemySpawners;

    float delayTimer = 0;

    void Start()
    {
        enemySpawners = new(Object.FindObjectsOfType<EnemySpawner>());
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;
        if (currentEnemySpawnIndex < enemySpawnInfos.Count)
        {
            EnemySpawnInfo enemySpawnInfo = enemySpawnInfos[currentEnemySpawnIndex];

            if (delayTimer >= enemySpawnInfo.delayFromLast)
            {
                Debug.Log("spawned!");
                currentEnemySpawnIndex++;
                delayTimer = 0.0f;
            }
        }
    }
}

[System.Serializable]
struct EnemySpawnInfo
{
    [SerializeField] public float delayFromLast;  // how long (in seconds) to wait before the enemy spawns (since last spawn)
    [SerializeField] public int basicMobCount;    // how many basic mobs to spawn
}
