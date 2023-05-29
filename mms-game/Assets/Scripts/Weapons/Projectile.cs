using UnityEngine;

public abstract class Projectile : MonoBehaviour, DamageDealer
{
    [SerializeField] private float lifeLength = 10f;
    [SerializeField] private LayerMask surfaceToBounceOf;


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
            hittableInParent.Hit(transform.position, o, this);
        }
        else if (hittableInChild != null)
        {
            hittableInChild.Hit(transform.position, o, this);
        }
    }

    protected virtual void explode()
    {
        Destroy(gameObject);
    }

    // For
    protected virtual void OnCollisionEnter2D(Collision2D c)
    {
        Bounce();
        // explode();
    }

    public void Bounce()
    {
        Debug.DrawRay(transform.position, transform.right, Color.magenta, 10f);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 2f, surfaceToBounceOf);
        // ray.normal
        float angle = Vector2.SignedAngle(-transform.right, ray.normal);
        Debug.DrawRay(ray.point, ray.normal, Color.yellow, 10f);
        Debug.Log($"angle is {angle}, normal is {ray.normal}, right is {transform.right}");

        transform.Rotate(Vector3.forward * (180 + angle * 2));

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        HitOther(other.gameObject);
        explode();
    }
}