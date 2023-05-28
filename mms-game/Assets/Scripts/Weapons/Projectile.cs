using UnityEngine;

public abstract class Projectile : MonoBehaviour, DamageDealer
{
    [SerializeField] private float lifeLength = 10f;

    // Rotate or direct so that it points into the right direction
    public virtual void PointTo(float angle)
    {
        transform.Rotate(0, 0, angle);
    }

    // Start to fly
    public virtual void Fire()
    {
        gameObject.SetActive(true);
        Destroy(gameObject, lifeLength);
    }

    protected void HitOther(GameObject o)
    {
        Hittable hittableInParent = o.GetComponentInParent<Hittable>();
        Hittable hittableInChild = o.GetComponentInChildren<Hittable>();
        if (hittableInParent != null)
        {
            hittableInParent.Hit(transform.position, o.GetComponent<Collider2D>(), this);
        }
        else if (hittableInChild != null)
        {
            hittableInChild.Hit(transform.position, o.GetComponent<Collider2D>(), this);
        }
    }

    protected virtual void explode()
    {
        Destroy(gameObject);
    }

    // The default behaviour for when on collision enter
    protected virtual void OnCollisionEnter2D(Collision2D c)
    {
        HitOther(c.gameObject);
        explode();
    }
}