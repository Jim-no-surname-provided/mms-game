using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : ProjectileWeapon
{
    public override GameObject GetProjectile()
    {
        return Instantiate(projectilePrefab);
    }
}
