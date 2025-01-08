using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnvyAbility : MonoBehaviour
{
    [SerializeField] private SwitchWeapon switchWeapon;
    [SerializeField] private Texture2D crosshair = null;
    private bool isEnvyModeActive = false;
    private bool wasEnvyModeActive = false;
    private int enemyLayerMask;
    private int enemyBossLayerMask;
    private float abilityCooldown = 10.0f;  // Duration of the cooldown in seconds
    private float lastAbilityTime;
    private bool isCursorShown = false;
    private PlayerInventory playerInventory;



    private void Start()
    {
        playerInventory = PlayerInventory.Instance;
        enemyLayerMask = 1 << LayerMask.NameToLayer("Enemies");
        lastAbilityTime = -abilityCooldown;
        ResetCursorState();
    }

    private void Update()
    {
        HandleInput();
        if (isEnvyModeActive != wasEnvyModeActive)
        {
            if (isEnvyModeActive)
            {
                ShowCursor();
            }
            else
            {
                HideCursor();
            }
            wasEnvyModeActive = isEnvyModeActive;
        }
        if (GameManager.isGameOver || GameManager.isGamePaused)
        {
            ResetCursorState();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time - lastAbilityTime > abilityCooldown)
            {
                if (!isEnvyModeActive)
                {
                    isEnvyModeActive = true;
                }
                else
                {
                    ExecuteEnvyAbility();
                }
            }
        }

        if (!isEnvyModeActive && Time.time - lastAbilityTime < abilityCooldown && !GameManager.isGameOver && !GameManager.isGamePaused)
        {
            float remainingCooldown = abilityCooldown - (Time.time - lastAbilityTime);
            // Debug.Log(remainingCooldown.ToString("F2") + " seconds remaining until Envy ability is ready");
        }
    }



    private void ExecuteEnvyAbility()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, enemyLayerMask))
        {
            Weapon enemyWeapon = hit.collider.gameObject.GetComponentInChildren<Weapon>();

            if (enemyWeapon != null)
            {
                // Get the type of the enemy's weapon.
                SwitchWeapon.WeaponType enemyWeaponType = enemyWeapon.weaponType; // Assuming weapon script has weaponType

                // Activate the weapon that matches the enemy's weapon.
                foreach (SwitchWeapon.WeaponInfo playerWeaponInfo in switchWeapon.weapons)
                {
                    if (playerWeaponInfo.weaponType == enemyWeaponType)
                    {
                        playerWeaponInfo.isActivated = true;  // Activate the weapon
                        break;
                    }
                }

                // Switch to the weapon type that matches the enemy's weapon.
                switchWeapon.SwitchToWeaponType(enemyWeaponType);  // This method needs to be implemented in SwitchWeapon script

                GameObject playerWeaponObject = switchWeapon.getCrtWeapon();
                Weapon playerWeapon = playerWeaponObject.GetComponent<Weapon>();
                playerWeapon.magazineSize = enemyWeapon.magazineSize;
                playerWeapon.bulletsLeft = playerWeapon.magazineSize;
                playerWeapon.totalAmmo += enemyWeapon.magazineSize * 2;
                playerWeapon.canReload = true;
                isEnvyModeActive = false;
                lastAbilityTime = Time.time;
                playerInventory.EnvyAbilityUsed(playerWeapon.GetWeaponID());
                // Debug.Log("Weapon copied from enemy!");
            }
            else
            {
                // Debug.LogError("Enemy does not have a weapon component!");
                isEnvyModeActive = false;
            }
        }
        else
        {
            // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 3f);
            isEnvyModeActive = false;
        }
    }

    private void ShowCursor()
    {
        if (!isCursorShown)
        {
            Vector2 cursorOffset = new Vector2(crosshair.width / 2, crosshair.height / 2);
            Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
            isCursorShown = true;
        }
    }

    private void HideCursor()
    {
        if (isCursorShown)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            isCursorShown = false;
        }
    }

    private void ResetCursorState()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        isCursorShown = false;
        isEnvyModeActive = false;
        wasEnvyModeActive = false;
    }

}
