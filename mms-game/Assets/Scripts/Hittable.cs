using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hittable
{
    // It recieves the point it was hit at, the collider that got hit, and which weapon was used.
    void Hit(Vector3 hitPoint, GameObject target, DamageDealer weapon);
}