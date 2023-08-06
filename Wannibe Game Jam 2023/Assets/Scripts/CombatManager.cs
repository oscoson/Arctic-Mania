using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int mobCount;
    public int maxMobCount;
    public bool isFreezeTime;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    


    public void FreezeTime()
    {
        isFreezeTime = true;
        StartCoroutine(FreezeTiming(10));
    }

    private IEnumerator FreezeTiming(float freezeRate)
    {
        player.freezeAmount -= freezeRate;
        yield return new WaitForSecondsRealtime(1);
        if(player.freezeAmount > 0)
        {
            StartCoroutine(FreezeTiming(freezeRate));
        }
        else
        {
            isFreezeTime = false;
        }
    }

}
