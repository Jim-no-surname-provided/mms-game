using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected Transform firePosition;

    // A GameObject with a component of type Projectile. Every weapon can give a different projectile (for example a bullet, missile, bomb...)
    public abstract GameObject GetProjectile();

    // The position from which it fires. By default, it uses the position of the Transform given in firePosition, but it can be overwriten
    public virtual Vector3 GetFirePosition() => firePosition.position;

    // Override of the general use in Weapon. In this case it means to fire the weapon.
    public override void Use(float angle)
    {
        Projectile pr = GetProjectile().GetComponent<Projectile>();
        pr.gameObject.layer = gameObject.layer;
        pr.PointTo(angle);
        pr.transform.position = GetFirePosition();
        pr.gameObject.SetActive(true);
        pr.Fire();
    }
}
