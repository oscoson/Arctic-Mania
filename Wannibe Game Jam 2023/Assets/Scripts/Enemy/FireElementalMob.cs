using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementalMob : Mob
{
    private bool isFrozen;
    private Player player;
    private Rigidbody2D mobRB;
    private SpriteRenderer sprite;
    private float damageCooldown = 0f;
    private CombatManager combatManager;
    private GameObject target;


    private float chargeUpTimer = 0.0f;
    private float chargeUpThreshold = 1.0f;
    private float fireRadius = 5.0f;

    enum FireElementalMobState
    {
        Moving,
        Unfreezing,
    }

    FireElementalMobState mobState = FireElementalMobState.Moving;

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
        mobRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        isFrozen = false;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        combatManager = FindObjectOfType<CombatManager>();
        StartCoroutine(FindNewTargetCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        frost = health/maxHealth;
        switch (mobState)
        {
            case FireElementalMobState.Moving:
                if (target != null &&
                    (Vector3.Distance(target.transform.position, transform.position) < fireRadius * 0.5f))
                {
                    mobState = FireElementalMobState.Unfreezing;
                    // start playing charge up animation
                }
                break;
            case FireElementalMobState.Unfreezing:
                chargeUpTimer += Time.deltaTime;
                if (chargeUpTimer >= chargeUpThreshold && !isFrozen)
                {

                    // play fire storm animation
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fireRadius);
                    foreach (Collider2D collider in colliders)
                    {
                        IFreezable freezableEntity = collider.gameObject.GetComponent<IFreezable>();
                        freezableEntity?.UnFreeze();
                    }
                    mobState = FireElementalMobState.Moving;
                    FindNewTarget();
                    chargeUpTimer = 0.0f;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (mobState)
        {
            case FireElementalMobState.Moving:
                Move();
                break;
            case FireElementalMobState.Unfreezing:
                mobRB.MovePosition(mobRB.position);
                break;
        }
    }

    private void FindNewTarget()
    {
        Mob[] mobs = Object.FindObjectsOfType<Mob>();

        float closestDist = float.MaxValue;
        Mob closestMob = null;

        foreach (Mob mob in mobs)
        {
            IFreezable freezableEntity = (mob as IFreezable);
            if (freezableEntity is null) continue;
            if (!freezableEntity.IsFrozen()) continue;

            float dist = (transform.position - mob.transform.position).magnitude;
            if (dist < closestDist)
            {
                closestMob = mob;
            }
        }

        if (closestMob is null)
        {
            target = player.gameObject;
        }
        else
        {
            target = closestMob.gameObject;
        }
    }

    private IEnumerator FindNewTargetCoroutine()
    {
        while (true)
        {
            FindNewTarget();

            yield return new WaitForSeconds(1f);
        }
    }

    private void Move()
    {
        if (target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
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
        health = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;
        gameObject.layer = LayerMask.NameToLayer("Frozen");
        GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Frozen");
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(255, 166, 0, 255);
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
        health -= player.frostStrength * 0.05f;
        health = Mathf.Max(0, health);
        if(health == 0)
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
