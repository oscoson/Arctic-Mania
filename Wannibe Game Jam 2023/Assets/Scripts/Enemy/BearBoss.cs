using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBoss : MonoBehaviour
{
    private Player player;
    private Rigidbody2D bossRb;
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
                break;
            case BearBossPhaseState.Phase2:
                break;
            case BearBossPhaseState.Phase3:
                break;
            case BearBossPhaseState.Defeat:
                break;
        }
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
