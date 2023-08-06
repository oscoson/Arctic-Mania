using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    [SerializeField] bool willSpawn;
    [SerializeField] int spawnInterval;
    private CombatManager combatManager;

    // Start is called before the first frame update
    void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
    }

    void SpawnEnemy()
    {
        StartCoroutine(SpawnRate(spawnInterval));
    }

    bool GenerateRandomBool()
    {
        if (Random.value >= 0.8)
        {
            return true;
        }
        return false;
    }

    private IEnumerator SpawnRate(int waitTime)
    {
        willSpawn = GenerateRandomBool();
        yield return new WaitForSecondsRealtime(waitTime);
        if(combatManager.mobCount < combatManager.maxMobCount && willSpawn)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            combatManager.mobCount += 1;
            willSpawn = false;
        }
        StartCoroutine(SpawnRate(spawnInterval));
    }
}
