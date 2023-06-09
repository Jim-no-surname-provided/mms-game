using System.Collections;
using UnityEngine;

public class ShieldedSimpleMonster : SimpleMonster
{
    [SerializeField] private DetectionCollider shieldDetector;
    [SerializeField] private DetectionCollider weakPointDetector;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        weakPointDetector.onTriggerDetectionEvent += (x =>
        {
            Player player = x.GetComponent<Player>();
            if (player != null)
            {
                KillSimpleMonster();
            }
        });

        shieldDetector.onTriggerDetectionEvent += (x =>
        {
            Player player = x.GetComponent<Player>();
            if (player != null)
            {
                player.Hit();
            }
        });

        SetWallDetector();
    }

    public override void ChangeMovingDirection()
    {
        movingDirection = !movingDirection;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator KillMonsterAnimation ()
    {
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    public override void KillSimpleMonster()
    {
        StartCoroutine(KillMonsterAnimation());

    }
}
