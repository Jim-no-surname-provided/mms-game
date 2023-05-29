using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour, Hittable
{

    // This will change, but each one will be a weapon
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private Weapon[] weapons;
    private int nWeapons = 0;

    // This is the father of the weapons, and will be rotated instead of every Weapon individually
    [SerializeField] private Transform weaponPivot;

    // These two are for the input
    private PlayerInput playerInput;
    private InputAction fireAction;
    private InputAction resetAction;
    private InputAction mouseAction;

    // This will be used to point the weapon towards the mouse
    private float angle;

    private void Start()
    {
        // Getting references
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        // Input listening and firing
        fireAction = playerInput.actions["Fire"];
        mouseAction = playerInput.actions["MousePos"];
        resetAction = playerInput.actions["Reset"];

        fireAction.started += context => currentWeapon.Use(angle);
        mouseAction.performed += SetPointerPosition;
        resetAction.started += context => Die();

        // Get weapons
        weapons = GetComponentsInChildren<Weapon>();
        nWeapons = weapons.Length;
    }

    #region cursor
    private void Update()
    {
        UpdateCursor();
    }
    private Vector2 screenPos;

    private void SetPointerPosition(CallbackContext context)
    {
        screenPos = context.ReadValue<Vector2>();
    }

    public void UpdateCursor()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y));
        // TODO if there is a Crosshair, move it to there

        // It's negative because I don't know
        angle = -Vector2.SignedAngle(worldPos - weaponPivot.position, Vector2.right);
        weaponPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    private SpriteRenderer spriteRenderer;
    public void flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        currentWeapon.Flip();
    }

    #endregion

    #region weapons
    public void addWeapon(GameObject weaponPrefab)
    {
        if (!weaponPrefab.GetComponentInChildren<Weapon>())
        {
            return;
        }

        // This searches for the weapon inside the ones we already have
        int weaponIndex = GetWeaponIndexIfPresent(weaponPrefab);
        if (weaponIndex != -1)
        {
            SetCurrentWeapon(weapons[weaponIndex]);
            return;
        }


        if (nWeapons == weapons.Length)
        {
            DoubleWeaponsSize();
        }

        Weapon weapon = Instantiate(weaponPrefab, weaponPivot).GetComponent<Weapon>();
        weapons[nWeapons] = weapon;
        nWeapons++;

        SetCurrentWeapon(weapon);
    }

    private void SetCurrentWeapon(Weapon weapon)
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = weapon;
        weapon.gameObject.SetActive(true);
        weapon.Flip(spriteRenderer.flipX);
    }

    private int GetWeaponIndexIfPresent(GameObject weaponPrefab)
    {
        for (int i = 0; i < nWeapons; i++)
        {
            if (weapons[i].Equals(weaponPrefab.GetComponent<Weapon>()))
            {
                return i;
            }
        }
        return -1;
    }

    public void DoubleWeaponsSize()
    {
        Weapon[] newArray = new Weapon[weapons.Length * 2];
        for (int i = 0; i < weapons.Length; i++)
        {
            newArray[i] = weapons[i];
        }
        weapons = newArray;
    }
    #endregion

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

    #region Hit

    public void Hit(Vector3 hitPoint, GameObject target, DamageDealer weapon)
    {
        Hit();
    }
    public void Hit()
    {
        Die();
    }

    private void Die()
    {
        Debug.Log("Oooops you are dead :( \n Going to last Check point");
        TpToLastCheckPoint();
    }
    #endregion
}
