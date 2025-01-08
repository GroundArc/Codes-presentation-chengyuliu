using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class PlayerInventory : MonoBehaviour
{
    public int fragments { get; private set; }
    public int money { get; private set; }
    public int currentWeaponIndex { get; private set; }
    public string greedAbilityText { get; private set; }
    public int greedAbilityUsageCount { get; private set; }
    public string gluttonyAbilityText { get; private set; }
    public int gluttonyAbilityUsageCount { get; private set; }
    public string envyAbilityText { get; private set; }
    public int envyAbilityUsageCount { get; private set; }
    public string wrathAbilityText { get; private set; }
    public int wrathAbilityUsageCount { get; private set; }
    public string bossWeaponText { get; private set; }

    public string slothAbilityText { get; private set; }
    public int slothAbilityUsageCount { get; private set; }
    public string maximumUsageAbility { get; private set; }
    public string playerTitle { get; private set; }
    public UnityEvent OnFragmentsCollected;
    public UnityEvent OnMoneyCollected;
    public UnityEvent OnGreedAbilityGranted;
    public UnityEvent OnGluttonyAbilityGranted;
    public UnityEvent OnEnvyAbilityGranted;
    public UnityEvent onSlothAbilityGranted;
    public UnityEvent onWrathAbilityGranted;
    public UnityEvent onGreedAbilityUsed;
    public UnityEvent onGluttonyAbilityUsed;
    public UnityEvent onEnvyAbilityUsed;
    public UnityEvent onSlothAbilityUsed;
    public UnityEvent onWrathAbilityUsed;
    public UnityEvent onBossWeaponCollected;
    public UnityEvent OnAmmoExhausted;

    public static PlayerInventory Instance;
    public static event Action OnInstanceReady;

    private Weapon weapon;
    private SwitchWeapon switchWeapon;
    private GreedAbility greedAbility;
    private GluttonyAbility gluttonyAbility;
    private EnvyAbility envyAbility;
    private SlothAbility slothAbility;
    private WrathAbility wrathAbility;
    private BosswWeaponManager bosswWeaponManager;

    private int gluttonyAbilityIndex = 0;
    private int wrathAbilityIndex = 1;
    private int envyAbilityIndex = 2;
    private int slothAbilityIndex = 3;
    private float cooldownTime = 10f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            OnInstanceReady?.Invoke();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        weapon = GetComponent<Weapon>();
        greedAbility = GetComponent<GreedAbility>();
        if (greedAbility != null)
        {
            greedAbility.enabled = false;
        }
        gluttonyAbility = GetComponent<GluttonyAbility>();
        if (gluttonyAbility != null)
        {
            gluttonyAbility.enabled = false;
        }
        envyAbility = GetComponent<EnvyAbility>();
        if (envyAbility != null)
        {
            envyAbility.enabled = false;
        }
        slothAbility = GetComponent<SlothAbility>();
        if (slothAbility != null)
        {
            slothAbility.enabled = false;
        }
        wrathAbility = GetComponent<WrathAbility>();
        if (wrathAbility != null)
        {
            wrathAbility.enabled = false;
        }
        bosswWeaponManager = GetComponent<BosswWeaponManager>();

    }

    public void FragementsCollected()
    {
        GameSoundManager.Instance.PlayCollectFragmentSound();
        fragments++;
        OnFragmentsCollected.Invoke();
    }

    public void MoneyCollected(int amount)
    {
        money += amount;
        OnMoneyCollected.Invoke();
        //Debug.Log("Money: " + money);
    }

    public void GreedAbilityCollect()
    {
        GrantBribeAbility();
        greedAbilityText = "Bribe Ability(MB2): Bribe Enemies and Gain Allies\nPress 'SPACE' to See More Info.";
        OnGreedAbilityGranted.Invoke();
    }

    public void GrantBribeAbility()
    {
        Debug.Log("Bribe Ability Granted");
        if (greedAbility != null)
        {
            greedAbility.enabled = true;
        }
    }
    public void GluttonyAbilityCollect()
    {
        GrantGluttonyAbility();
        gluttonyAbilityText = "Gluttony Ability(Q): Swallow Enemies and Restoring Your Health\nPress 'SPACE' to See More Info.";
        OnGluttonyAbilityGranted.Invoke();
    }

    public void GrantGluttonyAbility()
    {
        Debug.Log("Gluttony Ability Granted");
        if (gluttonyAbility != null)
        {
            gluttonyAbility.enabled = true;
        }
    }
    public void EnvyAbilityCollect()
    {
        GrantEnvyAbility();
        envyAbilityText = "Envy Ability(E): Duplicate Weapon/Ammunition From Enemies\nPress 'SPACE' to See More Info.";
        OnEnvyAbilityGranted.Invoke();
    }
    public void GrantEnvyAbility()
    {
        Debug.Log("Envy Ability Granted");
        if (envyAbility != null)
        {
            envyAbility.enabled = true;
        }
    }
    public void SlothAbilityCollect()
    {
        GrantSlothAbility();
        slothAbilityText = "Sloth Ability(Z): Significantly Weaken Enemy Attributes\nPress 'SPACE' to See More Info.";
        onSlothAbilityGranted.Invoke();
    }
    public void GrantSlothAbility()
    {
        Debug.Log("Sloth Ability Granted");
        if (slothAbility != null)
        {
            slothAbility.enabled = true;
        }
    }
    public void WrathAbilityCollect()
    {
        GrantWrathAbility();
        wrathAbilityText = "Wrath Ability(X): Significantly Enhance Player's attributes\nPress 'SPACE' to See More Info.";
        onWrathAbilityGranted.Invoke();
    }
    public void GrantWrathAbility()
    {
        Debug.Log("Wrath Ability Granted");
        if (wrathAbility != null)
        {
            wrathAbility.enabled = true;
        }
    }

    public void GreedAbilityUsed()
    {
        GameSoundManager.Instance.PlayGreedAbilitySound();
        greedAbilityUsageCount++;
        onGreedAbilityUsed.Invoke();
    }

    public void GluttonyAbilityUsed()
    {
        GameSoundManager.Instance.PlayGluttonyAbilitySound();
        gluttonyAbilityUsageCount++;
        onGluttonyAbilityUsed.Invoke();
        InventoryUI.Instance.StartCooldown(gluttonyAbilityIndex, cooldownTime);
    }

    public void EnvyAbilityUsed(int weaponIndex)
    {
        GameSoundManager.Instance.PlayEnvyAbilitySound();
        WeaponActivated(weaponIndex);
        envyAbilityUsageCount++;
        onEnvyAbilityUsed.Invoke();
        InventoryUI.Instance.StartCooldown(envyAbilityIndex, cooldownTime);
    }

    public void SlothAbilityUsed()
    {
        GameSoundManager.Instance.PlaySlothAbilitySound();
        slothAbilityUsageCount++;
        onSlothAbilityUsed.Invoke();
        InventoryUI.Instance.StartCooldown(slothAbilityIndex, cooldownTime);
    }

    public void WrathAbilityUsed()
    {
        GameSoundManager.Instance.PlayWrathAbilitySound();
        wrathAbilityUsageCount++;
        onWrathAbilityUsed.Invoke();
        InventoryUI.Instance.StartCooldown(wrathAbilityIndex, cooldownTime);
    }

    public void SetPlayerTitle()
    {
        if (greedAbilityUsageCount == 0 && gluttonyAbilityUsageCount == 0 && envyAbilityUsageCount == 0 && slothAbilityUsageCount == 0 && wrathAbilityUsageCount == 0)
        {
            maximumUsageAbility = "You don't use any ability!";
            playerTitle = "Innocent";
            return;
        }
        int max = Math.Max(greedAbilityUsageCount, Math.Max(gluttonyAbilityUsageCount, Math.Max(envyAbilityUsageCount, Math.Max(slothAbilityUsageCount, wrathAbilityUsageCount))));
        if (max == greedAbilityUsageCount)
        {
            maximumUsageAbility = "The skill you used the most is Greed";
            playerTitle = "Greedy";
        }
        else if (max == gluttonyAbilityUsageCount)
        {
            maximumUsageAbility = "The skill you used the most is Gluttony";
            playerTitle = "Gluttonous";
        }
        else if (max == envyAbilityUsageCount)
        {
            maximumUsageAbility = "The skill you used the most is Envy";
            playerTitle = "Envious";
        }
        else if (max == slothAbilityUsageCount)
        {
            maximumUsageAbility = "The skill you used the most is Sloth";
            playerTitle = "Slothful";
        }
        else if (max == wrathAbilityUsageCount)
        {
            maximumUsageAbility = "The skill you used the most is Wrath";
            playerTitle = "Wrathful";
        }
    }
    public void BossWeaponCollected(int weaponIndex)
    {
        WeaponActivated(weaponIndex);
        Debug.Log("Boss Weapon Collected");
        onBossWeaponCollected.Invoke();
    }

    public void AmmoExhausted(int weaponIndex)
    {
        SetCurrentWeaponIndex(weaponIndex);
        OnAmmoExhausted.Invoke();
    }

    public void WeaponActivated(int weaponIndex)
    {
        // Debug.Log("Weapon Activated: " + weaponIndex);
        currentWeaponIndex = weaponIndex;
    }

    public void SetCurrentWeaponIndex(int weaponIndex)
    {
        // Debug.Log("Current Weapon Index: " + weaponIndex);
        currentWeaponIndex = weaponIndex;
    }
}
