using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCameraFollowPoint : MonoBehaviour
{
    BearBoss bearBoss;
    Player player;

    
    // Start is called before the first frame update
    void Start()
    {
        bearBoss = FindObjectOfType<BearBoss>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 bearBossPos = bearBoss.transform.position;
        Vector2 playerPos = player.transform.position;

        Vector2 midPoint = bearBossPos * 0.5f + playerPos * 0.5f;

        transform.position = midPoint;
    }
}
