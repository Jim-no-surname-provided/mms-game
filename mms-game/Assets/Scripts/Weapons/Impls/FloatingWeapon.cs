using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FloatingWeapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player p = other.GetComponent<Player>();

        if (p != null)
        {
            p.addWeapon(weaponPrefab);
            Destroy(gameObject);
        }
    }
}
