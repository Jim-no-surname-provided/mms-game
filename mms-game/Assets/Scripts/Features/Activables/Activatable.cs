using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    public void Toggle(bool on)
    {
        if (on)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }
    public abstract void TurnOn();
    public abstract void TurnOff();


}
