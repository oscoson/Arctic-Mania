using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowblower : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation = new Vector3(player.snowballSpawn.GetChild(0).transform.eulerAngles.x, 
                player.snowballSpawn.GetChild(0).transform.eulerAngles.y, player.snowballSpawn.GetChild(0).transform.eulerAngles.z);
        gameObject.transform.eulerAngles = newRotation;
    }

    void OnTriggerStay2D(Collider2D other)
    {
       GameObject collisionObject = other.gameObject;
        if(collisionObject.tag == "Enemy")
        {
            switch(collisionObject.name)
            {
                case "Basic Mob(Clone)":
                    BasicMob basicMob = collisionObject.GetComponent<BasicMob>();
                    basicMob.CheckFreezeSnowBlower();
                    break;
                case "ArcticSeal Mob(Clone)":
                    ArcticSealMob sealMob = collisionObject.GetComponent<ArcticSealMob>();
                    sealMob.CheckFreezeSnowBlower();
                    break;
                case "SnowHare Mob(Clone)":
                    SnowHareMob hareMob = collisionObject.GetComponent<SnowHareMob>();
                    hareMob.CheckFreezeSnowBlower();
                    break;
                case "FireElemental Mob(Clone)":
                    FireElementalMob fireMob = collisionObject.GetComponent<FireElementalMob>();
                    fireMob.CheckFreezeSnowBlower();
                    break;
                case "Fox Mob(Clone)":
                    FoxMob foxMob = collisionObject.GetComponent<FoxMob>();
                    foxMob.CheckFreezeSnowBlower();
                    break;
                case "HuskyMob(Clone)":
                    HuskyMob huskyMob = collisionObject.GetComponent<HuskyMob>();
                    huskyMob.CheckFreezeSnowBlower();
                    break;
            }

        }
        else if(collisionObject.tag == "Boss")
        {
            BearBoss bearBoss = collisionObject.GetComponent<BearBoss>();
            bearBoss.CheckFreezeSnowBlower();
        }
    }
}
