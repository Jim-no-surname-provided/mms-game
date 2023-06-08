using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUIText : MonoBehaviour
{
    public Text coinText;
    // Start is called before the first frame update
    void Start()
    {
        coinText.text = ItemCollector.coinCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
