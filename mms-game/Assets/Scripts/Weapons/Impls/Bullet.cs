using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : Projectile
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTrigger");
        Hittable hittableInParent = other.gameObject.GetComponentInParent<Hittable>();
        Hittable hittableInChild = other.gameObject.GetComponentInChildren<Hittable>();
        if (hittableInParent != null)
        {
            hittableInParent.Hit(transform.position, GetComponent<Collider2D>(), this);
        }
        else if (hittableInChild != null)
        {
            hittableInChild.Hit(transform.position, GetComponent<Collider2D>(), this);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        Debug.Log("OnCollision");
        Hittable hittableInParent = c.gameObject.GetComponentInParent<Hittable>();
        Hittable hittableInChild = c.gameObject.GetComponentInChildren<Hittable>();
        if (hittableInParent != null)
        {
            hittableInParent.Hit(transform.position, GetComponent<Collider2D>(), this);
        }
        else if (hittableInChild != null)
        {
            hittableInChild.Hit(transform.position, GetComponent<Collider2D>(), this);
        }

        Destroy(gameObject);
    }
}
