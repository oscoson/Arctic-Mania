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
    private List<GameObject> objectsHit = new List<GameObject>();
    
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

        StartCoroutine(RotateBoomerang());

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
        //Main enemy collision code
        GameObject collisionObject = other.gameObject;
        switch (collisionObject.tag)
        {
            case "Enemy":
                Mob mob = collisionObject.GetComponent<Mob>();
                if (!mob.IsFrozen() && !objectsHit.Contains(collisionObject))
                {
                    mob.CheckFreeze();
                    objectsHit.Add(collisionObject);
                    ReduceLife();
                }
                break;
            case "Player":
                if (invulnerabilityTime <= 0 && !objectsHit.Contains(collisionObject))
                {
                    //Also add the player to the list of objects hit so that the boomerang doesn't die twice, prevents permanent max boomerang increase
                    objectsHit.Add(collisionObject);
                    StartCoroutine(RemoveEnemyFromList(collisionObject));
                    DestroyBoomerang();
                }
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Handles the case where the boomerang is stuck in the player
        GameObject collisionObject = other.gameObject;
        if (collisionObject.tag == "Player")
        {
            if (invulnerabilityTime <= 0)
            {
                DestroyBoomerang();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Prevents the boomerang from hitting the same enemy twice instantly
        GameObject collisionObject = other.gameObject;
        if (collisionObject.tag == "Enemy")
        {
            StartCoroutine(RemoveEnemyFromList(collisionObject));
        }
    }

    private IEnumerator RemoveEnemyFromList(GameObject enemy)
    {
        yield return new WaitForSeconds(0.5f);
        objectsHit.Remove(enemy);
    }

    private IEnumerator RotateBoomerang()
    {
        while (true)
        {
            transform.Rotate(0, 0, 10);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void DestroyBoomerang()
    {
        activeBoomerangs--;
        activeBoomerangs = Mathf.Max(0, activeBoomerangs);
        Destroy(gameObject);
    }

    public float GetInvulnerability()
    {
        return invulnerabilityTime;
    }

    private void ReduceLife()
    {
        life--;
        if (life <= 0)
        {
            DestroyBoomerang();
        }
    }

}
