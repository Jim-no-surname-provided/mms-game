using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static bool boughtGun = false;
    public static bool boughtBouncingGun = false;
    public static bool boughtFreezingGun = false;
    public GameObject buyError;
    public GameObject boughtError;
    public GameObject curr;
    public GameObject shoppingWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyGun() 
    {
        if(ItemCollector.coinCount < 50 && !boughtGun) 
        {
            buyError.SetActive(true);
        }
        else if(!boughtGun)
        {
            boughtGun = true;
            ItemCollector.coinCount = ItemCollector.coinCount - 50;
            shoppingWindow.SetActive(false);
        }
    }

    public void boughtGunError() 
    {
        if(boughtGun)
        {
            boughtError.SetActive(true);
            curr.SetActive(false);
        }
    }



    public void buyBouncingGun() 
    {
        if(ItemCollector.coinCount < 200 && !boughtBouncingGun) 
        {
            buyError.SetActive(true);
            curr.SetActive(false);
        }
        else if(!boughtBouncingGun)
        {
            boughtBouncingGun = true;
            ItemCollector.coinCount = ItemCollector.coinCount - 200;
            shoppingWindow.SetActive(false);
        }
    }

    public void boughtBouncingGunError() 
    {
        if(boughtBouncingGun)
        {
            boughtError.SetActive(true);
        }
    }



    public void buyFreezingGun() 
    {
        if(ItemCollector.coinCount < 500 && !boughtFreezingGun) 
        {
            buyError.SetActive(true);
            curr.SetActive(false);
        }
        else if(!boughtFreezingGun)
        {
            boughtFreezingGun = true;
            ItemCollector.coinCount = ItemCollector.coinCount - 500;
            shoppingWindow.SetActive(false);
        }
    }

    public void boughtFreezingGunError() 
    {
        if(boughtFreezingGun)
        {
            boughtError.SetActive(true);
        }
    }
}
