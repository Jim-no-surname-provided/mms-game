using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMovement : Enemy
{
    [SerializeField] private bool movingDirection = false;
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate (((movingDirection ? -1 : 1)* Vector3.left * Time.deltaTime * speed));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TpToLastCheckPoint(); //kill player 
        }
        else if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy" )
        {
            movingDirection = !movingDirection;
        }
    } 

    void OnTriggerEnter2D(Collider2D trigger){
        if (trigger.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }

    }

    public override void Hit(Vector3 hitPoint, Collider2D collider, DamageDealer weapon)
    {
       Destroy(this.gameObject);
    }
}
