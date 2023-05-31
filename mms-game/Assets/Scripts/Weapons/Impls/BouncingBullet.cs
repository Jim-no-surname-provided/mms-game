using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BouncingBullet : BouncingProjectile
{
    private void FixedUpdate()
    {
        Move();
    }

    [SerializeField]
    private float speed = 30f;
    private void Move()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}