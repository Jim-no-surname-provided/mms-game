using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour, Hittable
{

    // This will change, but each one will be a weapon
    [SerializeField]
    private Weapon currentWeapon;

    // This is the father of the weapons, and will be rotated instead of every Weapon individually
    [SerializeField]
    private Transform weaponPivot;

    // These two are for the input
    private PlayerInput playerInput;
    private InputAction fireAction;
    private InputAction mouseAction;

    // This will be used to point the weapon towards the mouse
    private float angle;

    private void Start()
    {
        // Input listening and firing
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions["Fire"];
        mouseAction = playerInput.actions["MousePos"];
        fireAction.started += (context) => currentWeapon.Use(angle);
        mouseAction.performed += SetPointerPosition;
    }
    private Vector2 screenPos;

    private void SetPointerPosition(CallbackContext context)
    {
        screenPos = context.ReadValue<Vector2>();
        UpdateCursor();
    }

    public void UpdateCursor()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y));
        // TODO if there is a Crosshair, move it to there

        // It's negative because I don't know
        angle = -Vector2.SignedAngle(worldPos - weaponPivot.position, Vector2.right);
        weaponPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    #region Checkpoints
    [SerializeField]
    private Vector3 lastCheckPoint = Vector3.zero;
    public void TpToLastCheckPoint()
    {
        transform.position = lastCheckPoint;
    }

    public void SetCheckPoint(Vector3 checkPointPos)
    {
        lastCheckPoint = checkPointPos;
    }

    #endregion

    public void Hit(Vector3 hitPoint, GameObject target, DamageDealer weapon)
    {
        Hit();
    }

    public void Hit(){
        Debug.Log("Oooops you are dead :( \n Going to last Check point");
        TpToLastCheckPoint();
    }
}
