using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class GluttonyAbility : MonoBehaviour
{
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private Texture2D crosshair = null;
    [SerializeField] private LayerMask groundMask;
    private bool isGluttonyModeActive = false;
    private bool wasGluttonyModeActive = false;

    private int enemyLayerMask;
    private float abilityCooldown = 10.0f;  // Duration of the cooldown in seconds
    private float lastAbilityTime;
    private bool isCursorShown = false;
    private PlayerInventory playerInventory;



    private void Start()
    {
        playerInventory = PlayerInventory.Instance;
        // Get the main camera for aiming purposes
        groundMask = LayerMask.GetMask("Ground");
        enemyLayerMask = 1 << LayerMask.NameToLayer("Enemies");
        lastAbilityTime = -abilityCooldown;
        ResetCursorState();
    }
    private void Update()
    {
        HandleInput();
        if (isGluttonyModeActive != wasGluttonyModeActive)
        {
            if (isGluttonyModeActive)
            {
                ShowCursor();
            }
            else
            {
                HideCursor();
            }
            wasGluttonyModeActive = isGluttonyModeActive;
        }
        if (GameManager.isGameOver || GameManager.isGamePaused)
        {
            ResetCursorState();
        }
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time - lastAbilityTime > abilityCooldown)
            {
                if (!isGluttonyModeActive)
                {
                    isGluttonyModeActive = true;
                }
                else
                {
                    ExecuteGluttonyAbility();
                }
            }
        }
        if (!isGluttonyModeActive && Time.time - lastAbilityTime < abilityCooldown && !GameManager.isGameOver && !GameManager.isGamePaused)
        {
            float remainingCooldown = abilityCooldown - (Time.time - lastAbilityTime);
            // Debug.Log(remainingCooldown.ToString("F2") + " seconds remaining until Gluttony ability is ready");
        }
    }

    private void ExecuteGluttonyAbility()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, enemyLayerMask))
        {
            EnemyInventory enemyInventory = hit.collider.gameObject.GetComponent<EnemyInventory>();
            enemyInventory.TakenGluttonyAbility();
            Destroy(hit.collider.gameObject);
            healthManager.RestoreFullHealth();
            isGluttonyModeActive = false;
            lastAbilityTime = Time.time;
            playerInventory.GluttonyAbilityUsed();
            // Debug.Log("Enemy killed and health restored");
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 3f);
            isGluttonyModeActive = false;
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
        isGluttonyModeActive = false;
        wasGluttonyModeActive = false;
    }

}