using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3f;

    [SerializeField]
    private GameObject[] pickupItems = new GameObject[4];

    private Player player;

    private GameObject item;
    private Sprite sprite;


    private void Start()
    {
        item = pickupItems[GenerateRandomNum()];
        player = FindObjectOfType<Player>();
        gameObject.GetComponent<SpriteRenderer>().sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false; // Disable collider so that it doesn't get picked up again 
        StartCoroutine(AnimateItemPickup());
    }


    int GenerateRandomNum()
    {
        return Random.Range(0, pickupItems.Length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        
        if(collisionObject.tag == "Player")
        {   
            // Debug.Log("itme picked up: " + item.name);
            player.CheckSnowblower();
            player.projectiles[player.currentProjectileIndex] = item;
            DestroyItem();
        }
    }

    private IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = 
                Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
