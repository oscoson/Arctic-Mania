using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mob : MonoBehaviour, IFreezable
{
    public EnemySO mob;
    protected float health;
    protected float speed;
    protected float frost;
    protected float damage;

    public abstract bool IsFrozen();
    public abstract void UnFreeze();
    public abstract void Freeze();
}

public enum EnemyID
{
    BasicMob = 0,
    FireElementalMob,
    SnowHareMob,
    ArcticSealMob,
};

