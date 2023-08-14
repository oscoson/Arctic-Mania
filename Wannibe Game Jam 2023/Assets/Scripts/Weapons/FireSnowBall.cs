using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSnowBall : MonoBehaviour
{
    [Header("Attributes")]
    private Rigidbody2D magicRB;
    private Player player;
    private Vector3 mousePos;
    private Camera mainCam;
    private GameObject audioManager;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] float xSpeed;
    [SerializeField] AudioClip snowballHit;
    [SerializeField] AudioClip snowballFire;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager");
        audioManager.GetComponent<AudioManager>().PlaySFX(snowballFire);
        magicRB = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - player.transform.position;
        Vector3 rotation = transform.position - mousePos;
        magicRB.velocity = new Vector2(direction.x, direction.y).normalized * xSpeed; //normalized so that ball stays at a constant speed no matter how far mouse is from player
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg; //make a degree float
        transform.rotation = Quaternion.Euler(0, 0, rot - 180);
        

        //transform.localScale = new Vector2(player.GetComponent<Transform>().localScale.x, 1f);
    }


    void Update()
    {
        //magicRB.velocity = new Vector2(xSpeed, 0f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject collisionObject = other.gameObject;
        if(collisionObject.tag == "Enemy")
        {
            audioManager.GetComponent<AudioManager>().PlaySFX(snowballHit);
            Mob mob = collisionObject.GetComponent<Mob>();
            mob.CheckFreeze();
        }
        else if(collisionObject.tag == "Boss")
        {
            audioManager.GetComponent<AudioManager>().PlaySFX(snowballHit);
            BearBoss boss = collisionObject.GetComponent<BearBoss>();
            if(boss.GetTotalBossHealth() > 0)
            {
                boss.DealDamage((int)player.frostStrength);
            }
        }
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
    }
}
