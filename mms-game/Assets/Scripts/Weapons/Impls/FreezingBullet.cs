using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingBullet : Bullet
{
    [SerializeField] private float freezeDuration = 2f;

    public float FreezeDuration => freezeDuration;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        HitOther(other.gameObject);

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                IFreezable freezable = enemy as IFreezable;
                if (freezable != null)
                {
                    freezable.Freeze(freezeDuration);
                }
            }
        }

        explode();
    }
}

