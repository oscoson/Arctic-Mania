using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIcicle : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int destroyTime = 5;
    private Rigidbody2D magicRB;
    private Player player;
    private Vector3 mousePos;
    private Camera mainCam;

    [SerializeField] float xSpeed;

    void Start()
    {
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
        StartCoroutine(DestructionTime(destroyTime));
    }


    void Update()
    {
        // magicRB.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;
        if(collisionObject.tag == "Enemy")
        {
            switch(collisionObject.name)
            {
                case "Basic Mob(Clone)":
                    BasicMob mob = collisionObject.GetComponent<BasicMob>();
                    mob.CheckFreeze();
                    break;
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestructionTime(int waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Destroy(gameObject);
    }
}
