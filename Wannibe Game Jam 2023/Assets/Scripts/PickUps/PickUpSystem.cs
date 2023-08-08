using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUpItem PickUpItem = collision.GetComponent<PickUpItem>();
        
        if(PickUpItem != null)
        {
            PickUpItem.DestroyItem();
        }
    }
}
