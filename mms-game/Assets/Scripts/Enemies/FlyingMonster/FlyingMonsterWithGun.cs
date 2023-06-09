using System.Collections;
using UnityEngine;

public class FlyingMonsterWithGun : SimpleMonster
{
    [SerializeField] private Gun gun;
    [SerializeField] private float shootingInterval = 3f;
    private Coroutine shootCoroutine;
    private GameObject player;

    protected override void Start()
    {
        base.Start();
        shootCoroutine = StartCoroutine(ShootWeapon(gun));
        player = GameObject.FindGameObjectWithTag("Player");
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
    }
    
}
