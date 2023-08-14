using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFrostBar : MonoBehaviour
{
    private BearBoss boss;
    [SerializeField] private Image frostBar;
    [SerializeField] private Image backgroundFrostBar;

    public float currentHealth;
    [SerializeField] private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        boss = FindObjectOfType<BearBoss>();
        currentHealth = boss.GetTotalBossHealth();
        maxHealth = currentHealth;

    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = boss.GetCurrentHealth();
        frostBar.fillAmount = currentHealth / maxHealth;
    }

    void FixedUpdate()
    {
        if(backgroundFrostBar.fillAmount > frostBar.fillAmount)
        {
            backgroundFrostBar.fillAmount -= 0.001f;
        }
    }

}
