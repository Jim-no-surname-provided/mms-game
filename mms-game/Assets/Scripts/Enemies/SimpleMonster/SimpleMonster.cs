using UnityEngine;

public class SimpleMonster : Enemy, IFreezable
{
    [SerializeField] protected bool movingDirection = false;
    [SerializeField] float speed = 2.0f;

    [SerializeField] private DetectionCollider playerDetector;
    [SerializeField] private DetectionCollider wallDetector;
    protected Animator animator;

    protected SpriteRenderer spriteRenderer;
    [Range(0, 1)][SerializeField] private float eyeHightPercentage = 0.25f;

    private bool isFrozen = false;
    private float freezeDuration = 0f;
    private float remainingFreezeTime = 0f;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Create event listeners 
        //implement onTriggerDetectionEvent methods
        animator = GetComponent<Animator>();
        SetSpriteRenderer();

        playerDetector.onTriggerDetectionEvent += (x =>
        {
            Player player = x.GetComponent<Player>();
            if (player != null)
            {
                float eyeHight = transform.position.y + spriteRenderer.bounds.size.y * eyeHightPercentage;

                if (player.transform.position.y > eyeHight)
                {
                    KillSimpleMonster();
                }
                else
                {
                    player.Hit();
                }
            }
        });

        SetWallDetector();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isFrozen)
        {
            transform.Translate(((movingDirection ? -1 : 1) * Vector3.left * Time.deltaTime * speed));
        }
        else
        {
            remainingFreezeTime -= Time.deltaTime;
            if (remainingFreezeTime <= 0f)
            {
                Unfreeze();
            }
        }
    }

    public void SetSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetWallDetector()
    {
        wallDetector.onTriggerDetectionEvent += (x => ChangeMovingDirection());
    }

    public virtual void ChangeMovingDirection()
    {
        movingDirection = !movingDirection;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    public override void Hit(Vector3 hitPoint, GameObject target, DamageDealer weapon)
    {
        KillSimpleMonster();
    }

    public virtual void KillSimpleMonster()
    {
        Destroy(this.gameObject);
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeDuration = duration;
        remainingFreezeTime = duration;
    }

    public void Unfreeze()
    {
        isFrozen = false;
        freezeDuration = 0f;
        remainingFreezeTime = 0f;
    }
}