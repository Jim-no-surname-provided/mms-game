using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCollider : MonoBehaviour
{
    public delegate void OnTriggerDetectionEvent();
    public OnTriggerDetectionEvent onTriggerDetectionEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerDetectionEvent();
    }
}
