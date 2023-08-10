using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowHareMob : Mob
{
    private bool isFrozen;
    private Player player;
    private Rigidbody2D mobRB;
    private SpriteRenderer sprite;
    private float damageCooldown = 0f;
    private CombatManager combatManager;
    Vector3 target = Vector3.zero;

    private float sittingTimer = 0.0f;
    private float sittingThreshold = 0.25f;

    private float hopDistance = 4.0f;

    enum SnowHareMobState
    {
        Sitting,
        Hopping,
    }

    SnowHareMobState mobState = SnowHareMobState.Sitting;

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
            case SnowHareMobState.Sitting:
                sittingTimer += Time.deltaTime;
                if (sittingTimer >= sittingThreshold)
                {
                    sittingTimer = 0.0f;
                    mobState = SnowHareMobState.Hopping;
                    GetNewTarget();
                    StartCoroutine(HopCoroutine());
                }
                break;
            case SnowHareMobState.Hopping:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (mobState)
        {
            case SnowHareMobState.Sitting:
                mobRB.MovePosition(mobRB.position);  // basically, stay still
                break;
            case SnowHareMobState.Hopping:
                mobRB.velocity = (target - transform.position).normalized * speed;
                break;
        }
    }

    private IEnumerator HopCoroutine()
    {
        yield return new WaitForSeconds(hopDistance / speed);
        mobState = SnowHareMobState.Sitting;
    }

    private void GetNewTarget()
    {
        Vector3 direction = (player.transform.position - transform.position);

        if (direction.magnitude < hopDistance)
        {
            target = player.transform.position;
            return;
        }

        direction = direction.normalized * hopDistance;

        target = Quaternion.Euler(0, 0, (Random.value < 0.5 ? -1 : 1) * Random.Range(20, 60)) * direction + transform.position;
    }

    private void MovePosition(Vector2 direction)
    {
        // As frost value goes down, speed decreases
        mobRB.MovePosition((Vector2)transform.position + (direction * (speed * (frost)) * Time.deltaTime));
    }

    public override void Freeze()
    {
        frost = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(200, 200, 255, 255);
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
