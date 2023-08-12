using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickUps : MonoBehaviour
{

    public Player player;
    public Color darkcolor;
    [SerializeField] private Sprite snowblower;
    [SerializeField] private Sprite snowball;
    [SerializeField] private Sprite icicle;
    [SerializeField] private Sprite boomerang;

    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    
    void Update()
    {
        if(player.currentProjectileIndex == 0){
            // If selected item is item 1:
            setImage(player.projectiles[0].name, 0);
            setImage(player.projectiles[1].name, 1);
            image2.color = darkcolor;
            image1.color = Color.white;
        } else {
            // If selected item is item 2:
            setImage(player.projectiles[0].name, 0);
            setImage(player.projectiles[1].name, 1);
            image1.color = darkcolor;
            image2.color = Color.white;
        }
        
    }

    void setImage(string name, int index)
    {
        if(index == 0){
            // IF this is the 1st image
            if(name == "Snowblower"){
                image1.sprite = snowblower;
            } else if(name == "Snowball"){
                image1.sprite = snowball;
            } else if(name == "Icicle"){
                image1.sprite = icicle;
            } else if(name == "Boomerang"){
                image1.sprite = boomerang;
            }
        } else {
            // IF this is the 2nd image
            if(name == "Snowblower"){
                image2.sprite = snowblower;
            } else if(name == "Snowball"){
                image2.sprite = snowball;
            } else if(name == "Icicle"){
                image2.sprite = icicle;
            } else if(name == "Boomerang"){
                image2.sprite = boomerang;
            }
        }
    }
}
