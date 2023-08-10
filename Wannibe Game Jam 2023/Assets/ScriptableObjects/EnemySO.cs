using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacter", menuName = " New Enemy Character")]
public class EnemySO : ScriptableObject
{
    public float health;
    public float maxHealth;
    public float speed;
    public float damage;
}
