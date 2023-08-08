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
}
