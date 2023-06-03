using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public int coinCount = 0; // number of coins

    private void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject); // remove the coin from the scene
            coinCount++; // increase the coin count
            Debug.Log("Collected a coin! Total coins: " + coinCount);
        }
    }
}
