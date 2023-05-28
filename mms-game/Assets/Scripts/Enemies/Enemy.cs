using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Hittable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(Vector3 hitPoint, Collider2D collider, Weapon weapon)
    {
        throw new System.NotImplementedException(); // TODO
    }

}
