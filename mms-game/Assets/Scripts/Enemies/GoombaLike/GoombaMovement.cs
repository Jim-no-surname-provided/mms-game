using UnityEngine;

public class GoombaMovement : Enemy
{
    [SerializeField] private bool movingDirection = false;
    [SerializeField] float speed;

    [SerializeField] private DetectionCollider killPlayer; 
    [SerializeField] private DetectionCollider wallDetector; 
    [SerializeField] private DetectionCollider killGoomba; 

    // Start is called before the first frame update
    void Start() {
        //Create event listeners 
        //implement onTriggerDetectionEvent methods

        killPlayer.onTriggerDetectionEvent += (x => {
            Player player = x.GetComponent<Player>();
            if (player != null){
                player.Hit(); 
            }
        });

        wallDetector.onTriggerDetectionEvent += (x => {
            movingDirection = !movingDirection;
        });

        killGoomba.onTriggerDetectionEvent += (x => {
            Player player = x.GetComponent<Player>();
            if (player != null){
                KillGoomba();
            }
        });

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate (((movingDirection ? -1 : 1)* Vector3.left * Time.deltaTime * speed));
    }


    public override void Hit(Vector3 hitPoint, Collider2D collider, DamageDealer weapon)
    {
       KillGoomba();
    }

    public void KillGoomba (){
        Destroy(this.gameObject);
    }
}
   /*  void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().tpToLastCheckPoint(); //kill player 
        }
        else if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy" )
        {
            movingDirection = !movingDirection;
        }
    }  */

    /*
    void OnTriggerEnter2D(Collider2D trigger){
        if (trigger.gameObject.GetComponent<DetectionCollider>().typeOfDetection == DetectionCollider.TypeOfDetection.DIE_FROM_PLAYER)
        {
            Destroy(this.gameObject); //kill Goomba
        }

    }
    */

