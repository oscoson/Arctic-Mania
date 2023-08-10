using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] List<EnemySpawnInfo> enemySpawnInfos;
    int currentEnemySpawnIndex = 0;  // the index for enemySpawnInfos

    List<EnemySpawner> enemySpawners;

    float delayTimer = 0.0f;

    void Start()
    {
        enemySpawners = new(Object.FindObjectsOfType<EnemySpawner>());
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;
        CheckPossibleSpawn();
    }

    private void CheckPossibleSpawn()
    {
        if (currentEnemySpawnIndex < enemySpawnInfos.Count)
        {
            EnemySpawnInfo enemySpawnInfo = enemySpawnInfos[currentEnemySpawnIndex];

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

}
