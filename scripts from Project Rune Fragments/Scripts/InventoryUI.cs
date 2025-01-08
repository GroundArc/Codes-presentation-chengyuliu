using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    public TextMeshProUGUI FragmentsText;
    private PlayerInventory playerInventory;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI AbilityText;
    public Image[] AbilityImage;
    public Image[] WeaponImage;
    [SerializeField] private Image[] cooldownMask;
    [SerializeField] private TextMeshProUGUI[] timeLeftText;
    private float[] cooldownTimes = new float[4] { 10f, 10f, 10f, 10f };
    private float[] timeLeft = new float[4];

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

    private void Start()
    {
        playerInventory = PlayerInventory.Instance;

        for (int i = 0; i < AbilityImage.Length; i++)
        {
            AbilityImage[i].gameObject.SetActive(false);
        }
        for (int i = 1; i < WeaponImage.Length; i++)
        {
            WeaponImage[i].gameObject.SetActive(false);
        }
        if (playerInventory != null && playerInventory.OnFragmentsCollected != null)
        {
            playerInventory.OnFragmentsCollected.AddListener(UpdateFragments);
        }
        if (playerInventory != null && playerInventory.OnMoneyCollected != null)
        {
            playerInventory.OnMoneyCollected.AddListener(UpdateMoney);
        }
        if (playerInventory != null && playerInventory.OnGreedAbilityGranted != null)
        {
            playerInventory.OnGreedAbilityGranted.AddListener(UpdateGreedAbility);
        }
        if (playerInventory != null && playerInventory.OnGluttonyAbilityGranted != null)
        {
            playerInventory.OnGluttonyAbilityGranted.AddListener(UpdateGluttonyAbility);
        }
        if (playerInventory != null && playerInventory.OnEnvyAbilityGranted != null)
        {
            playerInventory.OnEnvyAbilityGranted.AddListener(UpdateEnvyAbility);
        }
        if (playerInventory != null && playerInventory.onSlothAbilityGranted != null)
        {
            playerInventory.onSlothAbilityGranted.AddListener(UpdateSlothAbility);
        }
        if (playerInventory != null && playerInventory.onWrathAbilityGranted != null)
        {
            playerInventory.onWrathAbilityGranted.AddListener(UpdateWrathAbility);
        }
        if (playerInventory != null && playerInventory.onEnvyAbilityUsed != null)
        {
            playerInventory.onEnvyAbilityUsed.AddListener(UpdateWeapon);
        }
        if (playerInventory != null && playerInventory.onBossWeaponCollected != null)
        {
            playerInventory.onBossWeaponCollected.AddListener(UpdateWeapon);
        }
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < cooldownMask.Length; i++)
        {
            if (timeLeft[i] > 0)
            {
                timeLeft[i] -= Time.deltaTime;
                timeLeftText[i].text = Mathf.CeilToInt(timeLeft[i]).ToString();
                cooldownMask[i].fillAmount = timeLeft[i] / cooldownTimes[i];
            }
            else
            {
                timeLeftText[i].text = "";
                cooldownMask[i].fillAmount = 0;
            }
        }
    }

    public void StartCooldown(int abilityIndex, float cooldown)
    {
        if (abilityIndex >= 0 && abilityIndex < cooldownMask.Length)
        {
            cooldownTimes[abilityIndex] = cooldown;
            timeLeft[abilityIndex] = cooldown;
            cooldownMask[abilityIndex].fillAmount = 1;
        }
    }

    public void UpdateFragments()
    {
        FragmentsText.text = "[" + playerInventory.fragments.ToString() + "/5]";
    }

    public void UpdateMoney()
    {
        MoneyText.text = playerInventory.money.ToString();
    }

    public void UpdateGreedAbility()
    {
        Debug.Log("Update Greed Ability on Screen");
        CancelInvoke("HideAbilityText");
        AbilityText.text = playerInventory.greedAbilityText.ToString();
        AbilityImage[0].gameObject.SetActive(true);
        Invoke("HideAbilityText", 5f);
    }

    public void UpdateGluttonyAbility()
    {
        Debug.Log("Update Gluttony Ability on Screen");
        CancelInvoke("HideAbilityText");
        AbilityText.text = playerInventory.gluttonyAbilityText.ToString();
        AbilityImage[1].gameObject.SetActive(true);
        Invoke("HideAbilityText", 5f);
    }

    public void UpdateEnvyAbility()
    {
        Debug.Log("Update Envy Ability on Screen");
        CancelInvoke("HideAbilityText");
        AbilityText.text = playerInventory.envyAbilityText.ToString();
        AbilityImage[3].gameObject.SetActive(true);
        Invoke("HideAbilityText", 5f);
    }

    public void HideAbilityText()
    {
        AbilityText.text = "";
    }

    public void UpdateSlothAbility()
    {
        Debug.Log("Update Sloth Ability on Screen");
        CancelInvoke("HideAbilityText");
        AbilityText.text = playerInventory.slothAbilityText.ToString();
        AbilityImage[4].gameObject.SetActive(true);
        Invoke("HideAbilityText", 5f);
    }

    public void UpdateWrathAbility()
    {
        Debug.Log("Update Wrath Ability on Screen");
        CancelInvoke("HideAbilityText");
        AbilityText.text = playerInventory.wrathAbilityText.ToString();
        AbilityImage[2].gameObject.SetActive(true);
        Invoke("HideAbilityText", 5f);
    }

    public void UpdateWeapon()
    {
        Debug.Log("Update Weapon on Screen");
        int weaponIndex = playerInventory.currentWeaponIndex;
        if (WeaponImage[weaponIndex] != null)
        {
            WeaponImage[weaponIndex].gameObject.SetActive(true);
        }
    }
}
