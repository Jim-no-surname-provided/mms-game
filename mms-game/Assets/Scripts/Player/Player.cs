using UnityEngine;

public class Player : MonoBehaviour, Hittable
{


    #region Checkpoints
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

    #endregion
    
    public void Hit(Vector3 hitPoint, Collider2D collider, Weapon weapon)
    {
        throw new System.NotImplementedException(); // TODO
    }
}
