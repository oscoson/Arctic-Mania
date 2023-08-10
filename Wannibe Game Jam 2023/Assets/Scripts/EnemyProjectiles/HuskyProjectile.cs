using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskyProjectile : EnemyProjectile
{
    [SerializeField, Range(0f, 100f)] private float damage;
    [SerializeField, Range(0.1f, 100f)] private float lifetime;
    private float lifetimeTimer = 0.0f;

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player == null) return;

            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
