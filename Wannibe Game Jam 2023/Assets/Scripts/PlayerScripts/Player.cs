using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] PlayerSO stats;
    public float level;
    public float exp;
    public float health;
    public float maxHealth;
    public float speed;
    public float damage;
    public float freezeAmount; // freeze bar amount
    public float freezeMax; // max freeze bar points
    public float freezePoints; // freeze points come in i.e freeze one enemy = 10 etc etc
    public float freezeRate; // Rate at which freeze time decreases freeze amount
    public float frostStrength; // how strong the snowball frost is

    public int totalXP; // Player XP

    [Header("Projectiles")]
    [SerializeField] GameObject[] projectiles = new GameObject[5];
    public int currentProjectileIndex = 0;

    [Header("Transform Spawns/Checks")]
    public Transform snowballSpawn;

    // Private
    private InputSystem input = null;
    private Rigidbody2D playerRB = null;
    private Vector2 moveVector = Vector2.zero;
    private CombatManager combatManager;

    public UnityEvent onDeath;

    private void Awake()
    {
        input = new InputSystem();
        playerRB = GetComponent<Rigidbody2D>();
        level = stats.level;
        health = stats.health;
        speed = stats.speed;
        damage = stats.damage;
        freezeAmount = stats.freezeAmount;
        freezeMax = stats.freezeMax;
        freezePoints = stats.freezePoints;
        freezeRate = stats.freezeRate;
        frostStrength = stats.frostStrength; // frost strength ranges from 0.1-1

        combatManager = FindObjectOfType<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddXP(int xpAmount)
    {
        totalXP += xpAmount;
        Debug.Log("Collected " + xpAmount + " XP. Total XP: " + totalXP);
        // Add logic here for leveling up >>>>>>>>> @Oscar
    }
    public int GetTotalXP()
    {
        return totalXP;
    }


    private void FixedUpdate()
    {
        playerRB.velocity = moveVector * speed;
    }

    public void gainFreeze()
    {
        if (freezeAmount < 100)
        {
            if (freezeAmount + freezePoints > 100)
            {
                freezeAmount = freezeMax;
            }
            else
            {
                freezeAmount += freezePoints;
            }

        }

    }

    void OnFire(InputValue value)
    {
        Instantiate(projectiles[currentProjectileIndex], snowballSpawn.GetChild(0).position, Quaternion.Euler(0f, 180f, 0f));
    }

    void OnFreeze(InputValue value)
    {
        if (!combatManager.isFreezeTime && freezeAmount > 0)
            combatManager.FreezeTime();
        else if (combatManager.isFreezeTime && freezeAmount > 0)
        {
            combatManager.isFreezeTime = false;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            onDeath.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    // Movement
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
