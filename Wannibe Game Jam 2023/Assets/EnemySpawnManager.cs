using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public float intervalModifier = 1.0f;
    public float minSpawnInterval;
    public float interval;

    public Timer timer;

    void Update()
    {
        //Function to decrease spawn interval over time. Follows the one in the important text channel
        intervalModifier = (-(1 - minSpawnInterval) / timer.maxTime) * timer.GetTime() + 1;
    }
}
