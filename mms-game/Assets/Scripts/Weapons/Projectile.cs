using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    // Rotate or direct so that it points into the right direction
    public abstract void PointTo(Vector2 direction);

    // Start to fly
    public abstract void Fire();
}
