using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeProjectile : EnemyProjectile
{
    [SerializeField, Range(0f, 100f)] private float damage;
    [SerializeField, Range(0.1f, 100f)] private float lifetime;
    private float lifetimeTimer = 0.0f;

    private Vector2 target;
    private Vector2 start;
    private Vector2 perp;
    public override void Launch(Vector2 direction, float speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
        target = (Vector2) transform.position + direction.normalized * speed;
        start = transform.position;
        perp = Quaternion.Euler(0f, 0f, 90f) * (target - (Vector2)transform.position).normalized;
    }

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;
        transform.position = Vector2.LerpUnclamped(start, target, lifetimeTimer);
        transform.position += (Vector3) perp.normalized * Mathf.Sin(lifetimeTimer * 2 * Mathf.PI) * 2.0f;
        
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
