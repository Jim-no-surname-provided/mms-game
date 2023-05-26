using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, DamageDealer
{
    public abstract void Use(float angle);
}