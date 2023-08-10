using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayableCharacter", menuName = "New Playable Character")]
public class PlayerSO : ScriptableObject
{
    public float health;
    public float maxHealth;
    public float speed;
    public float damage;
    public float freezeAmount;
    public float freezeMax;
    public float freezePoints;
    public float freezeRate;
    public float frostStrength;
}
