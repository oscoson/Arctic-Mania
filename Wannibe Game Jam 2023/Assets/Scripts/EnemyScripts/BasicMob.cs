using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMob : Mob
{
    [SerializeField] bool isFrozen;
    private Player player;
    private Rigidbody2D mobRB;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        health = mob.health;
        speed = mob.speed;
        frost = mob.frost;
        damage = mob.damage;
        thawTime = mob.thawTime;
        player = FindObjectOfType<Player>();
        mobRB = this.GetComponent<Rigidbody2D>();
        sprite = this.GetComponent<SpriteRenderer>();
        isFrozen = false;
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
            MovePosition(direction);
        }
    }
    private void MovePosition(Vector2 direction)
    {
        // As frost value goes down, speed decreases
        mobRB.MovePosition((Vector2)transform.position + (direction * (speed * (frost)) * Time.deltaTime));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject collisionObject = other.gameObject;
        if(collisionObject.tag == "Snowball")
        {
            if(frost > 0 && !isFrozen)
            {
                frost -= player.frostStrength;
                if(frost <= 0)
                {
                    sprite.color = new Color(0, 149, 255, 255);
                    isFrozen = true;
                    player.gainFreeze();
                    StartCoroutine(Thaw(thawTime));
                }
            }

        }
    }

    private IEnumerator Thaw(float waitTime)
    {
        yield return new WaitForSecondsRealtime(thawTime);
        Debug.Log("Rest Frost");
        sprite.color = new Color(255, 0, 0, 255);
        frost = 1;
        isFrozen = false;
    }
}
