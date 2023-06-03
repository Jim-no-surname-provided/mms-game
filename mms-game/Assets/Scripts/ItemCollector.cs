using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int coinCount = 0; // number of coins

    [SerializeField] public Text coinText;

    private void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject); // remove the coin from the scene
            coinCount++; // increase the coin count
            coinText.text = "Coins: " + coinCount;
            Debug.Log("Collected a coin! Total coins: " + coinCount);
        }
    }
}
