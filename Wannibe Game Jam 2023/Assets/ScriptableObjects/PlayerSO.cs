using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayableCharacter", menuName = "New Playable Character")]
public class PlayerSO : ScriptableObject
{
    public float health;
    public float speed;
    public float damage;
    public float freezeAmount;
    public float freezeMax;
    public float freezeRate;
    public float freezeLength;
    public float frostStrength;
}
