using System.Collections;
using UnityEngine;

public class MonsterWithGun : SimpleMonster
{
    [SerializeField] private Gun gun;
    [SerializeField] private float shootingInterval = 3f; 
    [SerializeField] private float directionChangeInterval = 0f;
    private Coroutine shootCoroutine;
    private Coroutine directionChangeCoroutine;

    protected override void Start() 
    { 
        base.Start(); 
        shootCoroutine =  StartCoroutine(ShootWeapon(gun)); 

       /* if (directionChangeInterval > 0)
        {
            directionChangeCoroutine = StartCoroutine(ChangeDirectionCoroutine());
        }*/
    }

    IEnumerator ShootWeapon(Gun gun)
    {
        while (true)
        {
            animator.SetTrigger("shootTime");
            yield return new WaitForSeconds(1);
            gun.Use(movingDirection ? 0 : 180);
            yield return new WaitForSeconds(shootingInterval);
        }
    }

    IEnumerator ChangeDirectionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);
            ChangeMovingDirection();
        }
    }

    public override void ChangeMovingDirection()
    {
        Debug.Log("Changing direction");
        movingDirection = !movingDirection;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnDestroy()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }

        /*if (directionChangeCoroutine != null)
        {
            StopCoroutine(directionChangeCoroutine);
        }*/
    }
    
}
