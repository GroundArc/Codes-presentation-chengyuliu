using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    // Movement related components and variables

    #region Datamembers

    #region Editor Settings

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private ParticleSystemRenderer bloodSplatEffect;
    [SerializeField] private ParticleSystemRenderer healthRestoreEffect;
    [SerializeField] private ParticleSystemRenderer plunderBulletEffect;
    [SerializeField] private AudioClip playerGetHitSound;

    #endregion

    #region Private Fields

    private CharacterMovement _characterMovement;
    private Camera mainCamera;
    private bool isAlive;
    private ParticleSystem currentEffect;
    private TrailRenderer trailRenderer;

    #endregion

    #endregion


    #region Methods

    #region Unity Callbacks

    // Reference to the weapon switching component
    public SwitchWeapon switchWeapon;
    private Weapon weaponScript;

    private void Awake()
    {
        // Initialize character movement and set the character as alive
        _characterMovement = GetComponent<CharacterMovement>();
        trailRenderer = transform.Find("PlayerWrathTrail").GetComponent<TrailRenderer>();

        if (trailRenderer == null)
        {
            Debug.LogError("Trail renderer not found!");
        }
        trailRenderer.enabled = false;
        isAlive = true;
    }

    private void Start()
    {
        // Get the main camera for aiming purposes
        mainCamera = Camera.main;
        groundMask = LayerMask.GetMask("Ground");
        weaponScript = switchWeapon.crtWeapon.view.GetComponent<Weapon>();
    }

    void Update()
    {
        // Handle aiming, movement, and shooting
        if (!GameManager.isGameOver && !GameManager.isGamePaused)
        {
            var direction = Aim();
            UpdateMovementInput();
            // shootProjectile(direction);

            // Check for shooting input
            if (switchWeapon.crtWeapon.weaponType != SwitchWeapon.WeaponType.Deagle &&
                switchWeapon.crtWeapon.weaponType != SwitchWeapon.WeaponType.Pistol &&
                switchWeapon.crtWeapon.weaponType != SwitchWeapon.WeaponType.ShotGun)
            {
                if (Input.GetMouseButton(0))
                {
                    TryFireWithCurrentWeapon(direction);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TryFireWithCurrentWeapon(direction);
                }
            }

            UpdateWeaponScriptReference();

            if (Input.GetKeyDown(KeyCode.R))
            {
                weaponScript.Reload();
            }

            // destory particles
            if (currentEffect != null && !currentEffect.IsAlive())
            {
                Debug.Log("Destroying effect");
                Destroy(currentEffect.gameObject);
            }
        }
    }

    #endregion

    // Update the movement based on player's input
    private void UpdateMovementInput()
    {
        _characterMovement.SetMovementInput(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
    }


    // private void shootProjectile(Vector3 direction)
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         // Debug.Log(direction);
    //         Vector3 spawnPosition = transform.position + transform.forward * 1 + transform.up * 1;
    //         Quaternion rotation = Quaternion.LookRotation(direction);
    //         var projectile = Instantiate(projectilePrefab, spawnPosition, rotation);
    //     }
    // }

    // Calculate and return the aiming direction based on mouse position
    private Vector3 Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;
            // Ignore the height difference.
            direction.y = 0;
            // Make the transform look in the direction.
            transform.forward = direction;
            return direction;
        }
        return Vector3.zero;
    }

    // Get the mouse position in the world space
    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Debug.Log(ray);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }

    // Try to fire with the currently equipped weapon
    void TryFireWithCurrentWeapon(Vector3 direction)
    {
        //Debug.Log("Trying to fire with current weapon!");
        if (switchWeapon == null)
        {
            Debug.LogError("switchWeapon is null!");
            return;
        }

        if (switchWeapon.crtWeapon == null)
        {
            Debug.LogError("crtWeapon is null!");
            return;
        }

        if (switchWeapon.crtWeapon.view == null)
        {
            Debug.LogError("crtWeapon.view is null!");
            return;
        }

        UpdateWeaponScriptReference();
        if (weaponScript == null)
        {
            Debug.LogError("Weapon script not found on crtWeapon.view!");
            return;
        }

        weaponScript.TryFire(direction);

    }

    // Property to check if the character is alive
    public bool IsAlive
    {
        get { return this.isAlive; }
    }

    // Method to mark the character as dead
    public void Die()
    {
        this.isAlive = false;
    }

    public void TakeDamageEffect()
    {
        if (playerGetHitSound != null)
        {
            AudioSource.PlayClipAtPoint(playerGetHitSound, transform.position, 0.5f);
        }
        Vector3 effectPosition = new Vector3(transform.position.x, 1, transform.position.z);
        var particles = Instantiate(this.bloodSplatEffect, effectPosition, transform.rotation);
    }

    public void HealthRestoreEffect()
    {
        Vector3 effectPosition = new Vector3(transform.position.x, 1, transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(this.transform.up);
        currentEffect = Instantiate(this.healthRestoreEffect, effectPosition, rotation).gameObject.GetComponent<ParticleSystem>();
        currentEffect.transform.parent = this.transform;
    }

    public void PlunderBulletEffect()
    {
        Vector3 effectPosition = new Vector3(transform.position.x, 1.5f, transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(this.transform.up);
        currentEffect = Instantiate(this.plunderBulletEffect, effectPosition, rotation).gameObject.GetComponent<ParticleSystem>();
        currentEffect.transform.parent = this.transform;
    }

    public void WrathAbilityEffect()
    {
        trailRenderer.enabled = true;
    }

    public void WrathAbilityEffectOff()
    {
        trailRenderer.enabled = false;
    }

    private void UpdateWeaponScriptReference()
    {
        weaponScript = switchWeapon.crtWeapon.view.GetComponent<Weapon>();
    }


    #endregion
}
