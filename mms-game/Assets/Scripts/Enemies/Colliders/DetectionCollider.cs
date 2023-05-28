using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCollider : MonoBehaviour
{
    public enum TypeOfDetection
    {
        WALL_DETECTION,
        KILL_PLAYER,
        DIE_FROM_PLAYER
    }
    public TypeOfDetection typeOfDetection { get; private set; }

}
