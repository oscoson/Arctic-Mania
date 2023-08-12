using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProjectile : MonoBehaviour
{
    public virtual void Launch(Vector2 direction, float speed) { }
}
