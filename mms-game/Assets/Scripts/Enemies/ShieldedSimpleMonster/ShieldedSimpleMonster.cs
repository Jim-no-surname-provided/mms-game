using System.Collections;
using UnityEngine;

public class ShieldedSimpleMonster : SimpleMonster
{
    [SerializeField] private DetectionCollider shieldDetector;
    [SerializeField] private DetectionCollider weakPointDetector;
    [SerializeField] private float directionChangeInterval = 0f;
    private Coroutine directionChangeCoroutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        // SetSpriteRenderer();

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

        if (directionChangeInterval > 0)
        {
            directionChangeCoroutine = StartCoroutine(ChangeDirectionCoroutine());
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
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnDestroy()
    {
        if (directionChangeCoroutine != null)
        {
            StopCoroutine(directionChangeCoroutine);
        }
    }
}
