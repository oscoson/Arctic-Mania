using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacter", menuName = " New Enemy Character")]
public class EnemySO : ScriptableObject
{
    public float health;
    public float speed;
    public float frost;
    public float damage;
}
