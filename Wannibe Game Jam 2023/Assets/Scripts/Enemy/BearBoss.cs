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
        health = maxHealth[0];

        player = FindObjectOfType<Player>();
        bossRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Put non-physics based logic here
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
