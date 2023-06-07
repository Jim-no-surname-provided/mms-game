using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int coinCount = 0; // number of coins
    [SerializeField] private AudioSource coinCollect; //Audio

    [SerializeField] public Text coinText;

    private void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject); // remove the coin from the scene
            coinCollect.Play();
            coinCount++; // increase the coin count
            coinText.text = coinCount.ToString();
            Debug.Log("Collected a coin! Total coins: " + coinCount);
        }
    }
}
