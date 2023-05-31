using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezable
{
    void Freeze(float duration);
    void Unfreeze();
}