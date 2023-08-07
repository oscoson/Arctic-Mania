using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMob : Mob
{
    [SerializeField] bool isFrozen;
    [SerializeField] bool thaw;
    [SerializeField] bool frozenBeforeFreezeTime;
    private Player player;
    private Rigidbody2D mobRB;
    private SpriteRenderer sprite;
    private float damageCooldown = 0f;
    private CombatManager combatManager;

    [Header("Death Items")]
    [SerializeField] GameObject dropItem;
    [SerializeField] GameObject DropEXP;
    

    // Start is called before the first frame update
    void Awake()
    {
        health = mob.health;
        speed = mob.speed;
        frost = mob.frost;
        damage = mob.damage;
        thawTime = mob.thawTime;
        player = FindObjectOfType<Player>();
        mobRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        combatManager = FindObjectOfType<CombatManager>();
        isFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
        if(combatManager.isFreezeTime)
        {
            CheckFreezeInFreezeTime();
        }
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

    void UnFreeze()
    {
        thaw = false;
        frozenBeforeFreezeTime = false;
        sprite.color = new Color(255, 0, 0, 255);
        frost = 1;
        isFrozen = false;
    }

    public override void Freeze()
    {
        frost = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;
        if(thaw)
        {
            StartCoroutine(Thaw(thawTime));
        }
    }

    public override bool IsFrozen()
    {
        return isFrozen;
    }

    void CheckFreezeInFreezeTime()
    {
        // Freeze mob if freeze time and freeze amount still running
        if(player.freezeAmount > 0)
        {
            if (!frozenBeforeFreezeTime)
            {
                Freeze();
            }
            else
            {
                UnFreeze();
            }
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
        GameObject collisionObject = other.gameObject;
        if(collisionObject.tag == "Snowball")
        {
            // Make function for projectile freeze check?
            if(frost > 0 && !isFrozen && !combatManager.isFreezeTime)
            {
                frost -= player.frostStrength;
                if(frost <= 0)
                {
                    Freeze();
                    player.gainFreeze();
                    frozenBeforeFreezeTime = true;
                    thaw = true;
                    StartCoroutine(Thaw(thawTime));
                }
            }
            else if(combatManager.isFreezeTime && isFrozen)
            {
                Destroy(gameObject);
                Instantiate(DropEXP, transform.position, Quaternion.identity);

                // This is for spawning the death items
                bool willSpawnitem = GenerateRandomBool();
                if (willSpawnitem)
                {
                    Instantiate(dropItem, transform.position, Quaternion.identity);
                }
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
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

    private IEnumerator Thaw(float waitTime)
    {
        yield return new WaitForSecondsRealtime(thawTime);
        UnFreeze();
    }
}
