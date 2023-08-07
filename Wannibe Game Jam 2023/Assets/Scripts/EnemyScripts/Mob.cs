using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mob : MonoBehaviour, IFreezable
{
    public EnemySO mob;
    public float health;
    public float speed;
    public float frost;
    public float damage;
    public float thawTime;

    public abstract bool IsFrozen();

    public abstract void Freeze();
}

