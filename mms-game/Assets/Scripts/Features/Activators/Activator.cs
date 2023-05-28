using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activator : MonoBehaviour, Hittable
{
    public abstract void Hit(Vector3 hitPoint, GameObject target, DamageDealer weapon);
    [SerializeField]
    protected Activatable[] activatables;

    public void Toggle(bool on)
    {
        foreach (Activatable activatable in activatables)
        {
            activatable.Toggle(on);
        }
    }
    public void TurnOn() => Toggle(true);
    public void TurnOff() => Toggle(false);


}
