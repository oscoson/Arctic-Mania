using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMob : Mob
{
    private bool isFrozen;
    private Player player;
    private Rigidbody2D mobRB;
    private SpriteRenderer sprite;
    private float damageCooldown = 0f;
    private CombatManager combatManager;

    [Header("Death Items")]
    [SerializeField] GameObject dropItem;
    private float dropSpawnChance;

    FoxMobState mobState = FoxMobState.Moving;


    float chargeUpTime = 1.5f;

    float initialChargeSpeed = 30.0f;
    float speedCap = 30.0f;  // should be the same as initialChargeSpeed
    float acc = 60.0f;

    private float roamTimer = 0.0f;
    private float roamThreshold = 3.0f;

    enum FoxMobState
    {
        Moving,
        ReadyingCharge,
        Charging,
    }

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
        isFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        frost = health / maxHealth;
        switch (mobState)
        {
            case FoxMobState.Moving:
                if(!isFrozen)
                {
                    roamTimer += Time.deltaTime;
                    if (roamTimer >= roamThreshold && (player.transform.position - transform.position).magnitude < 10.0f)
                    {
                        roamTimer = 0.0f;
                        mobState = FoxMobState.ReadyingCharge;
                        StartCoroutine(TransitionToCharging());
                    }
                }
                break;
            case FoxMobState.ReadyingCharge:
                break;
            case FoxMobState.Charging:
                break;
        }
    }

    private void FixedUpdate()
    {
        if(!isFrozen)
        {
            switch (mobState)
            {
                case FoxMobState.Moving:
                    Move();
                    break;
                case FoxMobState.ReadyingCharge:
                    mobRB.MovePosition(transform.position);
                    break;
                case FoxMobState.Charging:
                    mobRB.velocity += (Vector2) (player.transform.position - transform.position).normalized * acc * Time.fixedDeltaTime;
                    if (mobRB.velocity.magnitude > speedCap && !isFrozen)
                    {
                        mobRB.velocity = mobRB.velocity.normalized * speedCap;
                    }
                    speedCap -= Time.fixedDeltaTime * 30.0f;
                    if (speedCap < 1.0f && !isFrozen)
                    {
                        speedCap = initialChargeSpeed;
                        mobState = FoxMobState.Moving;
                    }
                    break;
            }
        }
    }

    IEnumerator TransitionToCharging()
    {
        yield return new WaitForSeconds(chargeUpTime);
        if(!isFrozen)
        {
            mobRB.velocity = (player.transform.position - transform.position).normalized * initialChargeSpeed;
            mobState = FoxMobState.Charging;
        }
    }

    private void Move()
    {
        if (player != null)
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
        mobRB.MovePosition((Vector2)transform.position + (direction * (speed * frost) * Time.deltaTime));
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     GameObject collisionObject = other.gameObject;
    //     switch (collisionObject.tag)
    //     {
    //         case "Boomerang":
    //             if (!IsFrozen())
    //             {
    //                 collisionObject.GetComponent<Boomerang>().ReduceLife();
    //             }
    //             CheckFreeze();
    //             break;
    //     }
    // }

    public override void Freeze()
    {
        health = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;

        gameObject.layer = LayerMask.NameToLayer("Frozen");
        GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Frozen");
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(255, 0, 0, 255);
        health = maxHealth;
        isFrozen = false;

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
        if (frost > 0 && !isFrozen)
        {
            health -= player.frostStrength;
            if (health <= 0)
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
        if (frost == 0)
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
