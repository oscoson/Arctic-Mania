using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Boomerang : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    
    private bool isReturning = false;
    private Vector3 target;
    private Vector3 direction;
    private Vector3 velocity;
    private float distanceFromPlayer;
    private int life = 3;
    private static int activeBoomerangs = 0;
    
    public int maxActiveBoomerangs;
    public float speed;
    public float invulnerabilityTime;
    public float closeModifier;
    public float closeDistance;
    public float returnSpeedModifer;

    void Start()
    {

        if (activeBoomerangs >= maxActiveBoomerangs)
        {
            Destroy(gameObject);
        } else {
            activeBoomerangs++;
            //Debug.Log($"Increasing active boomerangs from {activeBoomerangs - 1} to {activeBoomerangs}");
        }

        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;
        direction = (target - transform.position).normalized;
        velocity = direction * speed;
        rb.velocity = new Vector2(velocity.x, velocity.y);
    }

    void Update()
    {

        if (invulnerabilityTime > 0) 
        {
            invulnerabilityTime -= Time.deltaTime;
        }

        if (!isReturning && Vector3.Distance(transform.position, target) < 0.5f)
        {
            isReturning = true;
        }
    }

    void FixedUpdate()
    {

        if (!isReturning)
        {
            return;
        }

        //Accelerate towards player, accelerate faster the closer the boomerang is to the player
        direction = (player.transform.position - transform.position).normalized;

        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer < closeDistance)
        {
            velocity += direction * speed * Time.deltaTime * closeModifier * returnSpeedModifer;
        } else {

            velocity += direction * speed * Time.deltaTime * returnSpeedModifer;
        }


        //cap the velocity
        if (velocity.magnitude > speed)
        {
            velocity = velocity.normalized * speed;
        }

        rb.velocity = new Vector2(velocity.x, velocity.y);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Boomerang collided with " + other.gameObject.name);
        GameObject collisionObject = other.gameObject;
        switch (collisionObject.tag)
        {
            case "Enemy":
                collisionObject.GetComponent<Mob>().CheckFreeze();
                ReduceLife();
                break;
            case "Player":
                if (invulnerabilityTime <= 0){
                    DestroyBoomerang();
                }
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;
        if (collisionObject.tag == "Player")
        {
            if (invulnerabilityTime <= 0)
            {
                DestroyBoomerang();
            }
        }
    }

    public void DestroyBoomerang()
    {
        Destroy(gameObject);
        activeBoomerangs--;
        activeBoomerangs = Mathf.Max(0, activeBoomerangs);
        //Debug.Log($"Decreasing active boomerangs from {activeBoomerangs + 1} to {activeBoomerangs}");
    }

    public float GetInvulnerability()
    {
        return invulnerabilityTime;
    }

    public void ReduceLife()
    {
        Debug.Log($"Reducing life from {life} to {life - 1}");
        life--;
        if (life <= 0)
        {
            Debug.Log("Destroying boomerang");
            DestroyBoomerang();
        }
    }

}
