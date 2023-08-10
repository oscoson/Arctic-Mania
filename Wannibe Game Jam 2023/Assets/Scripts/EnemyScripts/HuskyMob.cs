using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskyMob : Mob
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
    private float roamThreshold = 2.5f;

    private float chargeUpTimer = 0.0f;
    private float chargeUpThreshold = 0.5f;

    private float changeTargetTimer = 0.0f;
    private float changeTargetThreshold = 0.6f;

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
        speed = mob.speed;
        frost = mob.frost;
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
                    StartCoroutine(ShootProjectilesCoroutine());
                }
                break;
            case ArcticSealMobState.Shooting:
                break;
        }
    }

    IEnumerator ShootProjectilesCoroutine()
    {
        float delay = 0.05f;
        float startSpeed = 3.0f;
        float endSpeed = 15.0f;
        float deltaSpeed = 2.0f;


        for (float speed = startSpeed; speed <= endSpeed; speed += deltaSpeed)
        {
            Vector2 direction = Quaternion.Euler(0f, 0f, Random.Range(-10.0f, 10.0f)) * (player.transform.position - transform.position);

            GameObject go = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;  // shouldnt happen tbh
            rb.velocity = direction.normalized * speed;
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
            moveDirection = (target - (Vector2)transform.position).normalized;
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
        target = (Vector2)player.transform.position + randomVec;
    }

    private void MovePosition(Vector2 direction)
    {
        // As frost value goes down, speed decreases
        mobRB.MovePosition((Vector2)transform.position + (direction.normalized * (speed * (frost)) * Time.deltaTime));
    }

    public override void Freeze()
    {
        frost = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(238, 95, 255, 255);
        frost = 1;
        isFrozen = false;
    }

    public override bool IsFrozen()
    {
        return isFrozen;
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
