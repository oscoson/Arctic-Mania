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
        maxHealth = mob.maxHealth;
        speed = mob.speed;
        damage = mob.damage;
        dropItem = mob.dropItem;
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
            case SnowHareMobState.Sitting:
                sittingTimer += Time.deltaTime;
                if (sittingTimer >= sittingThreshold & !isFrozen)
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
                mobRB.velocity = (target - transform.position).normalized * (speed * frost);
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

    public override void Freeze()
    {
        health = 0;
        sprite.color = new Color(0, 149, 255, 255);
        isFrozen = true;
    }

    public override void UnFreeze()
    {
        sprite.color = new Color(200, 200, 255, 255);
        health = maxHealth;
        isFrozen = false;
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
    public override bool IsFrozen()
    {
        return isFrozen;
    }

    bool GenerateRandomBool()
    {
        if (Random.value >= 0.8)
        {
            return true;
        }
        return false;
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
