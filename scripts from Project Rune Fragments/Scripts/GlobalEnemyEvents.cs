using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyEvents : MonoBehaviour
{
    public static GlobalEnemyEvents Instance;

    public delegate void MovementSpeedChange(float newSpeed);
    public event MovementSpeedChange OnMovementSpeedChanged;

    public delegate void ProjectileDamageChangeWithMultiplier(float multiplier);
    public event ProjectileDamageChangeWithMultiplier OnProjectDamageChange;

    public delegate void ProjectileSpeedChangeWithMultiplier(float multiplier);
    public event ProjectileSpeedChangeWithMultiplier OnProjectileSpeedChange;

    public delegate void PlayerListUpdate();
    public event PlayerListUpdate OnPlayerListUpdate;

    public delegate void EnemyListUpdate();
    public event EnemyListUpdate OnEnemyListUpdate;

    public delegate void EnemyDeathEvent();
    public event EnemyDeathEvent OnEnemyDeath;

    private float currentMovementSpeed = 3.5f;
    private float currentProjectileSpeedMultiplier = 1f;
    private float currentProjectileDamageMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeGlobalMovementSpeed(float newSpeed)
    {
        currentMovementSpeed = newSpeed;
        OnMovementSpeedChanged?.Invoke(newSpeed);
    }

    public void ChangeGlobalDamageMultiplier(float multiplier)
    {
        currentProjectileDamageMultiplier = multiplier;
        OnProjectDamageChange?.Invoke(multiplier);
    }

    public void ChangeGlobalSpeedMutiplier(float multiplier)
    {
        currentProjectileSpeedMultiplier = multiplier;
        OnProjectileSpeedChange?.Invoke(multiplier);
    }

    public void UpdatePlayerList()
    {
        OnPlayerListUpdate?.Invoke();
    }

    public void UpdateEnemyList()
    {
        OnEnemyListUpdate?.Invoke();
    }

    public float GetCurrentMovementSpeed()
    {
        return currentMovementSpeed;
    }

    public float GetCurrentSpeedMultiplier()
    {
        return currentProjectileSpeedMultiplier;
    }

    public float GetCurrentDamageMultiplier()
    {
        return currentProjectileDamageMultiplier;
    }

    public void EnemyDied()
    {
        OnEnemyDeath?.Invoke();
    }
}
