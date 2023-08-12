using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateProjectile : EnemyProjectile
{
    [SerializeField, Range(0f, 100f)] private float damage;
    [SerializeField, Range(0.1f, 100f)] private float lifetime;
    [SerializeField, Range(-100f, 100f)] private float acceleration;
    private float lifetimeTimer = 0.0f;

    public override void Launch(Vector2 direction, float speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
    }

    public void SetAcceleration(float acc)
    {
        acceleration = acc;
    }

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float speed = rb.velocity.magnitude;
        speed += acceleration * Time.fixedDeltaTime;
        rb.velocity = rb.velocity.normalized * speed;
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
