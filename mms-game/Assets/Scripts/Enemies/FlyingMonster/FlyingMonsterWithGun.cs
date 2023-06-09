using System.Collections;
using UnityEngine;

public class FlyingMonsterWithGun : SimpleMonster
{
    [SerializeField] private Gun gun;
    [SerializeField] private float shootingInterval = 3f;
    [SerializeField] private float directionChangeInterval = 0f;
    private Coroutine shootCoroutine;
    private Coroutine directionChangeCoroutine;
    private GameObject player;

    protected override void Start()
    {
        base.Start();
        shootCoroutine = StartCoroutine(ShootWeapon(gun));
        player = GameObject.FindGameObjectWithTag("Player");

        if (directionChangeInterval > 0)
        {
            directionChangeCoroutine = StartCoroutine(ChangeDirectionCoroutine());
        }
    }

    IEnumerator ShootWeapon(Gun gun)
    {
        while (true)
        {
            if (player != null)
            {
                Vector3 directionToPlayer = player.transform.position - transform.position;
                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                gun.Use(angle);
            }
            
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
        movingDirection = !movingDirection;
        //spriteRenderer.flipX = !spriteRenderer.flipX;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnDestroy()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }

        if (directionChangeCoroutine != null)
        {
            StopCoroutine(directionChangeCoroutine);
        }
    }
    
}
