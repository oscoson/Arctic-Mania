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

    [Header("Death Items")]
    [SerializeField] GameObject dropItem;

    // Start is called before the first frame update
    void Awake()
    {
        health = mob.health;
        maxHealth = mob.maxHealth;
        speed = mob.speed;
        damage = mob.damage;
        player = FindObjectOfType<Player>();
        mobRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        combatManager = FindObjectOfType<CombatManager>();
        isFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        frost = health/maxHealth;
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
            float angle = Mathf.Atan2(direction.y, direction.x);
            mobRB.rotation = angle;
            direction.Normalize();
            MovePosition(direction);
        }
    }
    private void MovePosition(Vector2 direction)
    {
        // As frost value goes down, speed decreases
        mobRB.MovePosition((Vector2)transform.position + (direction * (speed * (frost)) * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;
        switch (collisionObject.tag)
        {
            case "Boomerang":
                if (!IsFrozen()){
                    collisionObject.GetComponent<Boomerang>().ReduceLife();
                }
                CheckFreeze();
            break;
        }
    }

    public override void Freeze()
    {
        health = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;

        gameObject.layer = LayerMask.NameToLayer("Frozen");
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(255, 0, 0, 255);
        health = maxHealth;
        isFrozen = false;

        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    public override bool IsFrozen()
    {
        return isFrozen;
    }

    public void CheckFreeze()
    {
        // Make function for projectile freeze check?
        if(frost > 0 && !isFrozen)
        {
            frost -= player.frostStrength;
            if(frost <= 0)
            {
                Freeze();
            }
        }
    }

    public void CheckFreezeSnowBlower()
    {
        // Brian this is bad but I had no other choice
        frost -= player.frostStrength * 0.05f;
        frost = Mathf.Max(0, frost);
        if(frost == 0)
        {
            Freeze();
        }
    }

    bool GenerateRandomBool()
    {
        if (Random.value >= 0.8)
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // GameObject collisionObject = other.gameObject;
        // if(collisionObject.tag == "Snowball")
        // {
        //     // Make function for projectile freeze check?
        //     if(frost > 0 && !isFrozen)
        //     {
        //         frost -= player.frostStrength;
        //         if(frost <= 0)
        //         {
        //             Freeze();
        //         }
        //     }

            // This is seperate from this^^ if uknow what i mean :P
            // else if(isFrozen)
            // {
            //     Destroy(gameObject);

            //     // This is for spawning the death items
            //     bool willSpawnitem = GenerateRandomBool();
            //     if (willSpawnitem)
            //     {
            //         Instantiate(dropItem, transform.position, Quaternion.identity);
            //     }
            // }
        // }
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
