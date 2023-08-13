using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] PlayerSO stats;
    [SerializeField] private CoolDownTimer cooldown;

    public float health;
    public float maxHealth;
    public float speed;
    public float damage;
    public float freezeAmount; // freeze bar amount
    public float freezeMax; // max freeze bar points
    public float freezePoints; // freeze points come in i.e freeze one enemy = 10 etc etc
    public float freezeRate; // Rate at which freeze time decreases freeze amount
    public float frostStrength; // how strong the snowball frost is


    [Header("Projectiles")]
    [SerializeField] public GameObject[] projectiles = new GameObject[2];
    [SerializeField] public int currentProjectileIndex;

    [Header("Transform Spawns/Checks")]
    public Transform snowballSpawn;

    // Private
    private InputSystem input = null;
    private Rigidbody2D playerRB = null;
    private Vector2 moveVector = Vector2.zero;
    private Animator playerAnimator;
    private CombatManager combatManager;

    public UnityEvent onDeath;

    private void Awake()
    {
        input = new InputSystem();
        playerRB = GetComponent<Rigidbody2D>();
        health = stats.health;
        maxHealth = stats.maxHealth;
        speed = stats.speed;
        damage = stats.damage;
        freezeAmount = stats.freezeAmount;
        freezeMax = stats.freezeMax;
        freezePoints = stats.freezePoints;
        freezeRate = stats.freezeRate;
        frostStrength = stats.frostStrength; // frost strength ranges from 0.1-1
        playerAnimator = GetComponent<Animator>();
        combatManager = FindObjectOfType<CombatManager>();

        StartCoroutine(PrintBoomerangs());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        playerRB.velocity = moveVector * speed;
    }

    private IEnumerator PrintBoomerangs()
    {
        while (true)
        {
            //Wait for 0.25 seconds
            yield return new WaitForSeconds(0.25f);
            Debug.Log($"Active boomerangs: {Boomerang.activeBoomerangs}");
        }
    }

    void OnFire(InputValue value)
    {
        
        if(cooldown.isCoolingDown) return;

        if(projectiles[currentProjectileIndex].name == "Icicle")
        {
            Instantiate(projectiles[currentProjectileIndex], snowballSpawn.GetChild(0).position, Quaternion.Euler(0f, 180f, 0f));
            cooldown.StartCoolDownIcy();
        }
        if(projectiles[currentProjectileIndex].name == "Snowball")
        {
             Instantiate(projectiles[currentProjectileIndex], snowballSpawn.GetChild(0).position, Quaternion.Euler(0f, 180f, 0f));
            cooldown.StartCoolDownSnowBall();
        }
        if(projectiles[currentProjectileIndex].name == "Boomerang") 
        {
            if (Boomerang.activeBoomerangs < Boomerang.maxActiveBoomerangs)
            {
                Instantiate(projectiles[currentProjectileIndex], snowballSpawn.GetChild(0).position, Quaternion.Euler(0f, 180f, 0f));
            }
        }
        
    }

    void OnFireHoldPerformed(InputAction.CallbackContext context)
    {
        if(projectiles[currentProjectileIndex].name == "Snowblower")
        {
            CheckSnowblower();
            Instantiate(projectiles[currentProjectileIndex], snowballSpawn.GetChild(0).position, Quaternion.Euler(0f, 0f, -90f), snowballSpawn.transform);
            FindAnyObjectByType<Snowblower>().GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void OnFireHoldCancelled(InputAction.CallbackContext context)
    {
        CheckSnowblower();
    }
    
    void OnSwitchPerformed(InputAction.CallbackContext context)
    {
        CheckSnowblower();
        if(currentProjectileIndex == 0)
        {
            currentProjectileIndex = 1;
        }
        else
        {
            currentProjectileIndex = 0;
        }
    }
    

    void OnSwitchCancelled(InputAction.CallbackContext context)
    {
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

    // Movement
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.FireHold.performed += OnFireHoldPerformed;
        input.Player.FireHold.canceled += OnFireHoldCancelled;
        input.Player.Switch.performed += OnSwitchPerformed;
        input.Player.Switch.canceled += OnSwitchCancelled;

    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        playerAnimator.SetBool("isRunning", true);
        moveVector = value.ReadValue<Vector2>();
        if(moveVector.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        playerAnimator.SetBool("isRunning", false);
        moveVector = Vector2.zero;
    }

    public void CheckSnowblower()
    {
        // Shouldn't really be here, but it's needed to prevent Snowblower instantiation errors
        if(GameObject.Find("Snowblower(Clone)") != null)
        {
            Destroy(FindObjectOfType<Snowblower>().gameObject);
        }
    }
}
