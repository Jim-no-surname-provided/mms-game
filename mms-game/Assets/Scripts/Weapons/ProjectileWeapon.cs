using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    // A GameObject with a component of type Projectile. Every weapon can give a different projectile (for example a bullet, missile, bomb...)
    public abstract GameObject GetProjectile();

    // The position from which 
    public abstract Vector3 GetFirePosition();
    
    // Override of the general use in Weapon. In this case it means to fire the weapon.
    public override void Use(Vector2 direction)
    {
        Projectile pr = GetProjectile().GetComponent<Projectile>();
        pr.PointTo(direction);
        pr.transform.position = GetFirePosition();
        pr.gameObject.SetActive(true);
        pr.Fire();
    }
}
