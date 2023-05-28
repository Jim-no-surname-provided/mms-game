using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCollider : MonoBehaviour
{
    public delegate void OnTriggerDetectionEvent(GameObject other);
    public OnTriggerDetectionEvent onTriggerDetectionEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerDetectionEvent(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        onTriggerDetectionEvent(other.gameObject);
    }
}
