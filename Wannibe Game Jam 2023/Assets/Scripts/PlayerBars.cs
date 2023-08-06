using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    public Image healthBar;
    public Image secondaryHealthBar;
    public Image FreezeBar;
    public Image secondaryFreezeBar;
    public Player player;
    [SerializeField] private float currentFreeze;
    [SerializeField] private float maxFreeze;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;


    private void Start()
    {
        maxHealth = player.health;
        Debug.Log(player.health);
    }

    private void Update()
    {
        currentHealth = player.health; // current health starts and updates with the player's health
        currentFreeze = player.freezeAmount;
        maxFreeze = player.freezeMax;
        healthBar.fillAmount = currentHealth / maxHealth;
        FreezeBar.fillAmount = currentFreeze / maxFreeze;
        

    }

    private void FixedUpdate()
    {
        if (secondaryHealthBar.fillAmount > healthBar.fillAmount)
        {
            secondaryHealthBar.fillAmount -= 0.01f;
        }
        // else match it with primary health bar
        else
        {
            secondaryHealthBar.fillAmount = healthBar.fillAmount;
        }

        if(secondaryFreezeBar.fillAmount > FreezeBar.fillAmount)
        {
            secondaryFreezeBar.fillAmount -= 0.01f;
        }
        else
        {
            secondaryFreezeBar.fillAmount = FreezeBar.fillAmount;
        }
    }
}
