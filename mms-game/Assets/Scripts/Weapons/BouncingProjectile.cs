using UnityEngine;

public abstract class BouncingProjectile : Projectile
{
    [SerializeField] private LayerMask surfaceToBounceOf;

    public void Bounce()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 2f, surfaceToBounceOf);
        float angle = Vector2.SignedAngle(-transform.right, ray.normal);
        transform.Rotate(Vector3.forward * (180 + angle * 2));
    }

    // For
    protected override void OnCollisionEnter2D(Collision2D c)
    {
        Bounce();
        //explode();
    }
}