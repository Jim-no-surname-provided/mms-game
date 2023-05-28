using UnityEngine;

public class GoombaMovement : Enemy
{
    [SerializeField] private bool movingDirection = false;
    [SerializeField] float speed;

    [SerializeField] private DetectionCollider playerDetector;
    [SerializeField] private DetectionCollider wallDetector;
    [Range(0, 1)][SerializeField] private float eyeHightPercentage = 0.8f;



    // Start is called before the first frame update
    void Start()
    {
        //Create event listeners 
        //implement onTriggerDetectionEvent methods

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        playerDetector.onTriggerDetectionEvent += (x =>
        {
            Player player = x.GetComponent<Player>();
            if (player != null)
            {
                float eyeHight = transform.position.y + spriteRenderer.bounds.size.y * eyeHightPercentage;

                if (player.transform.position.y > eyeHight)
                {
                    KillGoomba();
                }
                else
                {
                    player.Hit();
                }
            }
        });

        wallDetector.onTriggerDetectionEvent += (x =>
        {
            movingDirection = !movingDirection;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        });
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(((movingDirection ? -1 : 1) * Vector3.left * Time.deltaTime * speed));
    }


    public override void Hit(Vector3 hitPoint, GameObject target, DamageDealer weapon)
    {
        KillGoomba();
    }

    public void KillGoomba()
    {
        Destroy(this.gameObject);
    }
}