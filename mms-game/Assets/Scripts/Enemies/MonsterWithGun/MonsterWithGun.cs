using System.Collections;
using UnityEngine;

public class MonsterWithGun : SimpleMonster
{
    [SerializeField] private Gun gun;
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
            gun.Use(0);
            yield return new WaitForSeconds(2f);
        }
    }

    private void OnDestroy()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
    }
    
}
