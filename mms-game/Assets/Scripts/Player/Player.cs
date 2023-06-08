using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour, Hittable
{
    [SerializeField] public int health;
    [SerializeField] public int numOfHearts;
    //[SerializeField] public Text hitText;
    [SerializeField] public Image hitImage;


    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public DeathScreen DeathScreen;

    //audio
    [SerializeField] private AudioSource dieSound;
    [SerializeField] private AudioSource hitSound;

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
    private InputAction changeWeaponAction;
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
        changeWeaponAction = playerInput.actions["ChangeWeapon"];


        fireAction.started += context => currentWeapon.Use(angle);
        mouseAction.performed += SetPointerPosition;
        resetAction.started += context => Die();
        changeWeaponAction.started += context => ChangeToWeapon((int)context.ReadValue<float>());

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

    public void ChangeToWeapon(int index)
    {

        Debug.Log("The index is " + index);
        if (index > nWeapons)
        {
            return;
        }

        SetCurrentWeapon(weapons[index - 1]);
    }
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
        health--;
        CalcHearts();
        hitSound.Play(); //audio
        StartCoroutine(ShowAndHideHitImage(1));

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Oooops you are dead :( \n Going to last Check point");

        //StartCoroutine(ShowAndHideDeathText(3));

        dieSound.Play(); //audio
        DeathScreen.Death();

        //health = numOfHearts;
        //CalcHearts();

        //TpToLastCheckPoint();
    }

    private void CalcHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            } 
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    /*IEnumerator ShowAndHideDeathText(float delay)
        {
            dieText.gameObject.SetActive(true); // show death text
            yield return new WaitForSeconds(delay); // wait for specified delay
            dieText.gameObject.SetActive(false); // hide death text
        }*/

    IEnumerator ShowAndHideHitImage(float delay)
            {
                hitImage.gameObject.SetActive(true); // show death text
                yield return new WaitForSeconds(delay); // wait for specified delay
                hitImage.gameObject.SetActive(false); // hide death text
            }
    #endregion
}
