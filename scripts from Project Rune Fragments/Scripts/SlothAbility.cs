using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlothAbility : MonoBehaviour
{
    // [SerializeField] private UnityEvent onAbilityActivated;
    // [SerializeField] private UnityEvent onAbilityDeactivated;
    private bool isAbilityActive = false;
    private float abilityCooldown = 10f;
    private float abilityDuration = 10f;
    private float lastAbilityTime = -10f;
    private float normalMovementSpeed = 3.5f;
    private float reducedMovementSpeed = 0.5f;
    private float normalSpeedMultiplier = 1f;
    private float reducedSpeedMultiplier = 0.15f;
    private float normalDamageMultiplier = 1;
    private float reducedDamageMultiplier = 0.15f;
    private PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Time.time - lastAbilityTime > abilityCooldown)
        {
            ActivateWrathAbility();
        }

        if (isAbilityActive && Time.time - lastAbilityTime > abilityDuration)
        {
            DeactivateWrathAbility();
        }
    }

    private void ActivateWrathAbility()
    {
        isAbilityActive = true;
        lastAbilityTime = Time.time;
        GlobalEnemyEvents.Instance.ChangeGlobalMovementSpeed(reducedMovementSpeed);
        GlobalEnemyEvents.Instance.ChangeGlobalSpeedMutiplier(reducedSpeedMultiplier);
        GlobalEnemyEvents.Instance.ChangeGlobalDamageMultiplier(reducedDamageMultiplier);
        playerInventory.SlothAbilityUsed();
        // onAbilityActivated?.Invoke();
    }

    void DeactivateWrathAbility()
    {
        isAbilityActive = false;
        GlobalEnemyEvents.Instance.ChangeGlobalMovementSpeed(normalMovementSpeed);
        GlobalEnemyEvents.Instance.ChangeGlobalSpeedMutiplier(normalSpeedMultiplier);
        GlobalEnemyEvents.Instance.ChangeGlobalDamageMultiplier(normalDamageMultiplier);
        // onAbilityDeactivated?.Invoke();
    }
}