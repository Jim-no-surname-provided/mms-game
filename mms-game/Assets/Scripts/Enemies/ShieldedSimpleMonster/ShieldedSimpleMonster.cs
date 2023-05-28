using UnityEngine;

public class ShieldedSimpleMonster : SimpleMonster
{
    [SerializeField] private DetectionCollider shieldDetector;
    [SerializeField] private DetectionCollider weakPointDetector;

    // Start is called before the first frame update
    void Start()
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
    }

    public override void ChangeMovingDirection()
    {
        movingDirection = !movingDirection;
        transform.localScale = new Vector3(transform.localScale.x * -1 ,transform.localScale.y, transform.localScale.z );
    }

}
