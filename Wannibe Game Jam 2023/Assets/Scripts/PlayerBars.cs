using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    public Image healthBar;
    public Image secondaryHealthBar;

    public Player player;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;



    private void Start()
    {
        maxHealth = player.health;
    }

    private void Update()
    {
        currentHealth = player.health; // current health starts and updates with the player's health

        healthBar.fillAmount = currentHealth / maxHealth;

        

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

    }
}
