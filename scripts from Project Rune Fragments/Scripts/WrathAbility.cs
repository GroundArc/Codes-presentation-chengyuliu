using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WrathAbility : MonoBehaviour
{

    [SerializeField] private UnityEvent onWrathAbilityActivated;
    [SerializeField] private UnityEvent onWrathAbilityDeactivated;
    private bool isWrathAbilityActive = false;
    private float wrathAbilityCooldown = 10f;
    private float wrathAbilityDuration = 5f;
    private float lastWrathAbilityTime = -10f;
    private PlayerInventory playerInventory;
    public static float wrathMultiplier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Time.time - lastWrathAbilityTime > wrathAbilityCooldown)
        {
            ActivateWrathAbility();
        }

        if (isWrathAbilityActive && Time.time - lastWrathAbilityTime > wrathAbilityDuration)
        {
            DeactivateWrathAbility();
        }
    }

    private void ActivateWrathAbility()
    {
        wrathMultiplier = 2f;
        isWrathAbilityActive = true;
        lastWrathAbilityTime = Time.time;
        playerInventory.WrathAbilityUsed();
        onWrathAbilityActivated?.Invoke();
    }

    void DeactivateWrathAbility()
    {
        wrathMultiplier = 1f;
        isWrathAbilityActive = false;
        onWrathAbilityDeactivated?.Invoke();
    }

}
