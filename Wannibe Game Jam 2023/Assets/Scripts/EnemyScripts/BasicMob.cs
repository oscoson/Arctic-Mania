using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMob : Mob
{
    private Player player;
    private Rigidbody2D mobRB;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        health = mob.health;
        speed = mob.speed;
        frost = mob.frost;
        damage = mob.damage;
        player = FindObjectOfType<Player>();
        mobRB = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();

    }

    private void Move()
    {
        if(player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x);
            mobRB.rotation = angle;
            direction.Normalize();
            movement = direction;
            MovePosition(direction);
        }
    }
    private void MovePosition(Vector2 direction)
    {
        mobRB.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}
