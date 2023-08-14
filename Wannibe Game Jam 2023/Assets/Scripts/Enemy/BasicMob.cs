using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMob : Mob
{
    private bool isFrozen;
    private Player player;
    private Rigidbody2D mobRB;
    private SpriteRenderer sprite;
    private float damageCooldown = 0f;
    private CombatManager combatManager;
    private Animator animator;

    [Header("Death Items")]
    [SerializeField] GameObject dropItem;
    private float dropSpawnChance;

    // Start is called before the first frame update
    void Awake()
    {
        health = mob.health;
        maxHealth = mob.maxHealth;
        speed = mob.speed;
        damage = mob.damage;
        dropItem = mob.dropItem;
        dropSpawnChance = mob.dropSpawnRate;
        player = FindObjectOfType<Player>();
        mobRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        combatManager = FindObjectOfType<CombatManager>();
        animator = GetComponent<Animator>();
        isFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        frost = health / maxHealth;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            animator.SetFloat("Horizontal", direction.normalized.x * 10.0f);
            float angle = Mathf.Atan2(direction.y, direction.x);
            mobRB.rotation = angle;
            direction.Normalize();
            MovePosition(direction);
        }
    }
    private void MovePosition(Vector2 direction)
    {
        // As frost value goes down, speed decreases
        mobRB.MovePosition((Vector2)transform.position + (direction * (speed * frost) * Time.deltaTime));
    }


    public override void Freeze()
    {
        Drop();
        health = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;

        animator.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("Frozen");
        GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Frozen");
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(255, 255, 255, 255);
        health = maxHealth;
        isFrozen = false;

        animator.speed = 1.0f;
        animator.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Enemy");
        GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Enemy");
    }

    public override bool IsFrozen()
    {
        return isFrozen;
    }

    public override void CheckFreeze()
    {
        // Make function for projectile freeze check?
        if(frost > 0 && !isFrozen)
        {
            health -= player.frostStrength;
            if(health <= 0)
            {
                Freeze();
            }
        }
    }

    public override void Drop()
    {
        {
            // This is for spawning the death items
            bool willSpawnitem = GenerateRandomBool();
            if (willSpawnitem)
            {
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
        }
    }

    public void CheckFreezeSnowBlower()
    {
        // Brian this is bad but I had no other choice
        health -= player.frostStrength * 0.05f;
        health = Mathf.Max(0, health);
        if(frost == 0)
        {
            Freeze();
        }
    }

    bool GenerateRandomBool()
    {
        if (Random.value <= dropSpawnChance)
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        GameObject triggerObject = other.gameObject;
        if (other.CompareTag("Player"))
        {
            if (damageCooldown <= 0)
            {
                player.TakeDamage(damage);
                damageCooldown = 1f;
            }
            else
            {
                damageCooldown -= Time.deltaTime;
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            damageCooldown = 0f;
        }
    }
}
