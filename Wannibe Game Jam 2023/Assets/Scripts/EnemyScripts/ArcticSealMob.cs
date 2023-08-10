using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcticSealMob : Mob
{
    [SerializeField] EnemyProjectile projectile;

    private bool isFrozen;
    private Player player;
    private Rigidbody2D mobRB;
    private SpriteRenderer sprite;
    private float damageCooldown = 0f;
    private CombatManager combatManager;
    Vector2 target = Vector2.zero;

    private float roamTimer = 0.0f;
    private float roamThreshold = 3.0f;

    private float chargeUpTimer = 0.0f;
    private float chargeUpThreshold = 1.0f;

    private float changeTargetTimer = 0.0f;
    private float changeTargetThreshold = 1.5f;

    Vector2 moveDirection = Vector2.zero;

    enum ArcticSealMobState
    {
        Moving,
        ChargingUpShots,
        Shooting,
    }

    ArcticSealMobState mobState = ArcticSealMobState.Moving;

    [Header("Death Items")]
    [SerializeField] GameObject dropItem;

    // Start is called before the first frame update
    void Awake()
    {
        health = mob.health;
        maxHealth = mob.maxHealth;
        speed = mob.speed;
        damage = mob.damage;
        mobRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        isFrozen = false;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        combatManager = FindObjectOfType<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        frost = health/maxHealth;
        switch (mobState)
        {
            case ArcticSealMobState.Moving:
                roamTimer += Time.deltaTime;
                roamTimer = Mathf.Min(roamTimer, roamThreshold);
                if (roamTimer >= roamThreshold && Vector2.Distance(player.transform.position, transform.position) < 9.0f)
                {
                    mobState = ArcticSealMobState.ChargingUpShots;
                    roamTimer = 0.0f;
                }
                break;
            case ArcticSealMobState.ChargingUpShots:
                chargeUpTimer += Time.deltaTime;
                if (chargeUpTimer >= chargeUpThreshold)
                {
                    chargeUpTimer = 0.0f;
                    mobState = ArcticSealMobState.Shooting;
                    StartCoroutine(ShootProjectilesCoroutine(player.transform.position - transform.position));
                }
                break;
            case ArcticSealMobState.Shooting:
                break;
        }
    }

    IEnumerator ShootProjectilesCoroutine(Vector2 direction)
    {
        direction.Normalize();

        Vector2 originalDirection = direction;

        float angle = 45;
        float delay = 0.1f;
        float deltaAngle = 15;
        float speed = 6.0f;

        direction = Quaternion.Euler(0f, 0f, angle) * direction;

        for (int i = 0; i < (int)(angle / deltaAngle * 2); i++)
        {
            GameObject go = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;  // shouldnt happen tbh
            rb.velocity = direction.normalized * speed;
            direction = Quaternion.Euler(0f, 0f, -deltaAngle) * direction;
            yield return new WaitForSeconds(delay);
        }

        for (int i = 0; i < (int)(angle / deltaAngle * 2); i++)
        {
            GameObject go = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;  // shouldnt happen tbh
            rb.velocity = direction.normalized * speed;
            direction = Quaternion.Euler(0f, 0f, deltaAngle) * direction;
            yield return new WaitForSeconds(delay);
        }
        mobState = ArcticSealMobState.Moving;
    }

    private void FixedUpdate()
    {
        switch (mobState)
        {
            case ArcticSealMobState.Moving:
                Move();
                break;
            case ArcticSealMobState.ChargingUpShots:
                break;
            case ArcticSealMobState.Shooting:
                break;
        }
    }

    private void Move()
    {
        changeTargetTimer += Time.deltaTime;
        if (changeTargetTimer >= changeTargetThreshold)
        {
            GetNewTarget();
            changeTargetTimer = 0.0f;
            moveDirection = (target - (Vector2) transform.position).normalized;
        }
        MovePosition(moveDirection);
    }

    private void GetNewTarget()
    {
        float radius = 5.0f;
        Vector2 randomVec = Random.insideUnitCircle * radius;
        if (randomVec.magnitude < radius * 0.5f)
        {
            randomVec = randomVec.normalized * radius * 0.5f;
        }
        target = (Vector2) player.transform.position + randomVec;
    }

    private void MovePosition(Vector2 direction)
    {
        // As frost value goes down, speed decreases
        mobRB.MovePosition((Vector2)transform.position + (direction.normalized * (speed * frost) * Time.deltaTime));
    }

    public override void Freeze()
    {
        health = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(238, 95, 255, 255);
        health = maxHealth;
        isFrozen = false;
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

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    GameObject collisionObject = other.gameObject;
    //    if (collisionObject.tag == "Snowball")
    //    {
    //        // Make function for projectile freeze check?
    //        if (frost > 0 && !isFrozen)
    //        {
    //            frost -= player.frostStrength;
    //            if (frost <= 0)
    //            {
    //                Freeze();
    //            }
    //        }
    //        // else if(isFrozen)
    //        // {
    //        //     Destroy(gameObject);

    //        //     // This is for spawning the death items
    //        //     bool willSpawnitem = GenerateRandomBool();
    //        //     if (willSpawnitem)
    //        //     {
    //        //         Instantiate(dropItem, transform.position, Quaternion.identity);
    //        //     }
    //        // }
    //    }
    //}

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
}
