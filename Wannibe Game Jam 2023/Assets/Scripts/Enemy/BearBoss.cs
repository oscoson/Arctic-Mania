using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBoss : MonoBehaviour
{
    private Player player;
    private Rigidbody2D bossRb;
    private Animator animator;

    [SerializeField] EnemyProjectile slowToFastProjectile;
    [SerializeField] EnemyProjectile starPatternProjectile;
    [SerializeField] AccelerateProjectile accelerateProjectile;

    private float damageCooldown = 0f;

    public float bodyContactDamage = 20.0f;

    private const int totalPhases = 3; // needs to be same as number of enums for phaseXState, and vice versa
    private int currentPhase = 0;

    private readonly float[] maxHealth = new float[totalPhases]{ 100f, 100f, 100f };
    private float health;

    BearBossPhaseState bossPhaseState = BearBossPhaseState.Intro;
    Phase1State phase1State = Phase1State.Idle;
    Phase2State phase2State;
    Phase3State phase3State;

    /* variables for intro phase */
    Vector3 startScale = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 endScale = new Vector3(1.0f, 1.0f, 1.0f);

    float cosScaleAngle = 0.0f;  // angle needed to fake spinning
    float angleChangeSpeed = 360.0f * 2;
    int numberOfSpins = 10;

    float switchToPhase1Timer = 0.0f;
    float switchToPhase1Threshold = 1.0f;

    /* end intro variables */

    /* variables for phase 1 */
    float phase1Timer = 0.0f;  // generic timer reused for all phase1states

    float idleThreshold = 2.0f;

    bool performingPhase1Action = false;
    /* end phase1 variables */

    /* variables for phase 2 */

    /* end phase2 variables */

    /* variables for phase 3 */

    /* end phase3 variables */

    /* variables for defeat phase */

    /* end defeat variables */

    enum BearBossPhaseState
    {
        Intro,
        Phase1,
        Phase2,
        Phase3,
        Defeat,
    }

    enum Phase1State
    {
        Idle,
        MoveShoot,
        ChargePerpShoot,

    }

    enum Phase2State
    {
        // todo: add states for phase 2
    }

    enum Phase3State
    {
        // todo: add states for phase 3
    }

    void Awake()
    {
        player = FindObjectOfType<Player>();
        bossRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        health = maxHealth[0];
        bossPhaseState = BearBossPhaseState.Intro;
        transform.localScale = startScale;
    }

    void Update()
    {
        // Put non-physics based logic here
        switch (bossPhaseState)
        {
            case BearBossPhaseState.Intro:
                if (cosScaleAngle < (Mathf.PI * 2 * Mathf.Rad2Deg * numberOfSpins))
                {
                    cosScaleAngle += angleChangeSpeed * Time.deltaTime;
                    Vector3 scale = Vector3.Lerp(startScale, endScale, cosScaleAngle / (Mathf.PI * 2 * Mathf.Rad2Deg * numberOfSpins));
                    scale.x *= Mathf.Cos(cosScaleAngle * Mathf.Deg2Rad);
                    transform.localScale = scale;
                    break;
                }

                switchToPhase1Timer += Time.deltaTime;
                if (switchToPhase1Timer >= switchToPhase1Threshold)
                {
                    bossPhaseState = BearBossPhaseState.Phase1;
                }
                break;
            case BearBossPhaseState.Phase1:
                HandlePhase1Logic();
                break;
            case BearBossPhaseState.Phase2:
                break;
            case BearBossPhaseState.Phase3:
                break;
            case BearBossPhaseState.Defeat:
                break;
        }
    }

    private void FixedUpdate()
    {
        // Put physics based logic here
        switch (bossPhaseState)
        {
            case BearBossPhaseState.Intro:
                break;
            case BearBossPhaseState.Phase1:
                HandlePhase1FixedLogic();
                break;
            case BearBossPhaseState.Phase2:
                break;
            case BearBossPhaseState.Phase3:
                break;
            case BearBossPhaseState.Defeat:
                break;
        }
    }

    private void HandlePhase1Logic()
    {
        switch (phase1State)
        {
            case Phase1State.Idle:
                phase1Timer += Time.deltaTime;
                if (phase1Timer > idleThreshold)
                {
                    phase1Timer = 0.0f;
                    float rng = Random.value * 2;
                    if (rng <= 1)
                    {
                        animator.SetTrigger("StartWalking");
                        phase1State = Phase1State.MoveShoot;
                        StartCoroutine(ShootMoveCoroutine());
                    } else if (rng <= 2)
                    {
                        animator.SetTrigger("StartWalking");
                        phase1State = Phase1State.ChargePerpShoot;
                        StartCoroutine(ChargePerpShootCoroutine());
                    }
                }
                break;
            case Phase1State.MoveShoot:
                if (!performingPhase1Action)
                {
                    phase1State = Phase1State.Idle;
                }
                break;
            case Phase1State.ChargePerpShoot:
                if (!performingPhase1Action)
                {
                    phase1State = Phase1State.Idle;
                }
                break;
        }
    }

    private void HandlePhase1FixedLogic()
    {
        switch (phase1State)
        {
            case Phase1State.Idle:
                break;
            case Phase1State.MoveShoot:
                break;
            case Phase1State.ChargePerpShoot:
                break;
        }
    }

    IEnumerator ChargePerpShootCoroutine()
    {
        performingPhase1Action = true;

        for (int i = 0; i < 2; i++)
        {
            Vector2 direction = (player.transform.position - transform.position);

            direction = Quaternion.Euler(0f, 0f, (Random.value < 0.5f ? -1 : 1) * Random.Range(35f, 55f)) * direction;

            float chargeSpeed = 100.0f;

            float time = 1.0f;
            float timer = 0.0f;

            float shootTimer = 0.0f;
            float shootThreshold = 0.1f;
            float shootSpeed = 8.0f;

            float speedScale = 1.0f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                shootTimer += Time.deltaTime;
                Move(direction, chargeSpeed);

                if (shootTimer >= shootThreshold)
                {
                    shootTimer = 0.0f;
                    AccelerateProjectile proj1 = Instantiate(accelerateProjectile, transform.position, Quaternion.identity).GetComponent<AccelerateProjectile>();
                    proj1.SetAcceleration(10.0f * speedScale);
                    proj1.Launch(Quaternion.Euler(0f, 0f, 90f) * direction, shootSpeed * speedScale);
                    AccelerateProjectile proj3 = Instantiate(accelerateProjectile, transform.position, Quaternion.identity).GetComponent<AccelerateProjectile>();
                    proj3.SetAcceleration(10.0f * speedScale);
                    proj3.Launch(Quaternion.Euler(0f, 0f, -90f) * direction, shootSpeed * speedScale);

                    if (speedScale == 1.0f) speedScale = 0.5f;
                    else speedScale = 1.0f;
                }
                yield return null;
            }
        }

        animator.SetTrigger("StartIdle");
        performingPhase1Action = false;
    }

    IEnumerator ShootMoveCoroutine()
    {
        performingPhase1Action = true;


        for (int i = 0; i < 2; i++)
        {
            float shootTimer = 0.0f;
            float shootThreshold = 0.05f;
            float shootSpeed = 4.0f;
            float shootMaxSpeed = 35.0f;

            while (shootSpeed < shootMaxSpeed)
            {
                Move(player.transform.position - transform.position, 4.0f);

                shootTimer += Time.deltaTime;

                if (shootTimer >= shootThreshold)
                {
                    EnemyProjectile proj = Instantiate(slowToFastProjectile.gameObject, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                    proj.Launch(Quaternion.Euler(0.0f, 0.0f, Random.Range(-5.0f, 5.0f)) * (player.transform.position - transform.position), shootSpeed);
                    shootSpeed += 1.5f;
                    shootTimer = 0.0f;
                }
                yield return null;
            }
            
            for (float angle = 0; angle < Mathf.PI * 2 * Mathf.Rad2Deg; angle += 20.0f)
            {
                Move(player.transform.position - transform.position, 4.0f);

                float speed = Mathf.Cos((angle - Mathf.PI * Mathf.Rad2Deg) * Mathf.Deg2Rad) * 5.0f + 6.0f;
                EnemyProjectile proj = Instantiate(starPatternProjectile.gameObject, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                proj.Launch(Quaternion.Euler(0.0f, 0.0f, angle) * (player.transform.position - transform.position), speed);
                shootSpeed += 1.0f;
                shootTimer = 0.0f;
            }
            yield return new WaitForSeconds(1.0f);

            for (float angle = 0; angle < Mathf.PI * 2 * Mathf.Rad2Deg; angle += 20.0f)
            {
                Move(player.transform.position - transform.position, 4.0f);

                float speed = Mathf.Cos((angle - Mathf.PI * Mathf.Rad2Deg) * Mathf.Deg2Rad) * 3.0f + 6.0f + Random.Range(-2.0f, 2.0f);
                EnemyProjectile proj = Instantiate(starPatternProjectile.gameObject, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                proj.Launch(Quaternion.Euler(0.0f, 0.0f, angle) * (player.transform.position - transform.position), speed);
                shootSpeed += 1.0f;
                shootTimer = 0.0f;
            }

            float waitTimer = 0.0f;
            float waitThreshold = 1.0f;

            while (waitTimer < waitThreshold)
            {
                Move(player.transform.position - transform.position, 4.0f);

                waitTimer += Time.deltaTime;
                yield return null;
            }

        }
        animator.SetTrigger("StartIdle");
        performingPhase1Action = false;
    }

    public int GetTotalBossHealth()
    {
        int total = 0;
        foreach(int health in maxHealth)
        {
            total += health;
        }
        return total;
    }

    private void Move(Vector2 direction, float speed)
    {
        direction.Normalize();
        animator.SetFloat("Horizontal", direction.x);
        // As frost value goes down, speed decreases
        float frost = health / maxHealth[currentPhase];
        bossRb.MovePosition((Vector2)transform.position + (direction * (speed * frost) * Time.deltaTime));
    }

    public void CheckFreezeSnowBlower()
    {
        // Brian this is bad but I had no other choice
        health -= player.frostStrength * 0.05f;
        health = Mathf.Max(0, health);
        if (health == 0)
        {
            // die or next phase
        }
    }

    public void DealDamage(int damage)
    {
        switch (bossPhaseState)
        {
            case BearBossPhaseState.Intro:
                break;
            case BearBossPhaseState.Defeat:
                break;
            default:
                damage = Mathf.Max(0, damage);
                health -= damage;
                break;
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        GameObject triggerObject = other.gameObject;
        if (other.CompareTag("Player"))
        {
            if (damageCooldown <= 0)
            {
                player.TakeDamage(bodyContactDamage);
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