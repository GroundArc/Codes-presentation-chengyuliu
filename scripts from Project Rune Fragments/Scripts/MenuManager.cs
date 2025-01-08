using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject abilityInfoPanel;
    public GameObject weaponInfoPanel;
    public GameObject aboutGamePanel;
    public static bool isAbilityInfoPanelShown = false;
    public static bool isWeaponInfoPanelShown = false;
    public static bool isAboutGamePanelShown = false;
    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.Play();
        isAbilityInfoPanelShown = false;
        isWeaponInfoPanelShown = false;
        isAboutGamePanelShown = false;
        abilityInfoPanel.SetActive(false);
        weaponInfoPanel.SetActive(false);
        aboutGamePanel.SetActive(false);
    }

    public void ShowAbilityInfo()
    {
        isAbilityInfoPanelShown = true;
        abilityInfoPanel.SetActive(true);
    }

    public void HideAbilityInfo()
    {
        isAbilityInfoPanelShown = false;
        abilityInfoPanel.SetActive(false);
    }

    public void ShowWeaponInfo()
    {
        isWeaponInfoPanelShown = true;
        weaponInfoPanel.SetActive(true);
    }

    public void HideWeaponInfo()
    {
        isWeaponInfoPanelShown = false;
        weaponInfoPanel.SetActive(false);
    }

    public void ShowAboutGame()
    {
        isAboutGamePanelShown = true;
        aboutGamePanel.SetActive(true);
    }

    public void HideAboutGame()
    {
        isAboutGamePanelShown = false;
        aboutGamePanel.SetActive(false);
    }

    public void StartGame()
    {
        audioSource.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        audioSource.Stop();
        Application.Quit();
    }
}
