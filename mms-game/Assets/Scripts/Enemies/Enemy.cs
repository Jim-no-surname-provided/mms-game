using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Hittable, DamageDealer
{
    //what to do incase enemy is hit with a weapon
    public abstract void Hit(Vector3 hitPoint, GameObject target, DamageDealer weapon);
}
