using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 lastCheckPoint = Vector3.zero;

    public void tpToLastCheckPoint()
    {
        transform.position = lastCheckPoint;
    }

    public void setCheckPoint(Vector3 checkPointPos)
    {
        lastCheckPoint = checkPointPos;
    }
}
