using UnityEngine;

public abstract class Projectile : MonoBehaviour, DamageDealer
{
    [SerializeField]
    private float lifeLength = 10f;
    // // private float lifeBeginning;
    // Rotate or direct so that it points into the right direction
    public virtual void PointTo(float angle)
    {
        transform.Rotate(0, 0, angle);
    }

    // Start to fly
    public virtual void Fire()
    {
        gameObject.SetActive(true);
        // // lifeBeginning = Time.time;
        Destroy(gameObject, lifeLength);
    }

    // // protected void DieIfLifetimeEnded(){
    // //     if(Time.time - lifeBeginning > lifeLength){
    // //         Destroy(this);
    // //     }
    // // }
}