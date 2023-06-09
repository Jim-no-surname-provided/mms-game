using System.Collections;
using UnityEngine;

public class MonsterWithGun : SimpleMonster
{
    [SerializeField] private Gun gun;
    [SerializeField] private float shootingInterval = 3f; 
    private Coroutine shootCoroutine;

    protected override void Start() 
    { 
        base.Start(); 
        shootCoroutine =  StartCoroutine(ShootWeapon(gun)); 
    }

    IEnumerator ShootWeapon(Gun gun)
    {
        while (true)
        {
            gun.Use(movingDirection ? 180: 0);
            yield return new WaitForSeconds(shootingInterval);
        }
    }

    public override void ChangeMovingDirection()
    {
        movingDirection = !movingDirection;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnDestroy()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
    }
    
}
