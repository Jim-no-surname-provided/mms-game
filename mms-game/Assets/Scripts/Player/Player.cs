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

    private void SetPointerPosition(CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y));
        // TODO if there is a Crosshair, move it to there

        // It's negative because I don't know
        angle = -Vector2.SignedAngle(worldPos - transform.position, Vector2.right);
        weaponPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    #region Checkpoints
    [SerializeField]
    private Vector3 lastCheckPoint = Vector3.zero;
    public void tpToLastCheckPoint() // TODO Change names 
    {
        transform.position = lastCheckPoint;
    }

    public void setCheckPoint(Vector3 checkPointPos) // TODO Change names 
    {
        lastCheckPoint = checkPointPos;
    }

    #endregion

    public void Hit(Vector3 hitPoint, Collider2D collider, DamageDealer weapon)
    {
        throw new System.NotImplementedException(); // TODO
    }
}
