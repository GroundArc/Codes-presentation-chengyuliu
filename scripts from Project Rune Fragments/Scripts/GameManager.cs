using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject GameEndingPanel;
    public GameObject WinningEpiloguePanel;
    public GameObject LosingEpiloguePanel;
    public GameObject AbilityUsageCountPanel;
    public GameObject abilityInfoPanel;
    public GameObject weaponInfoPanel;
    public GameObject aboutGamePanel;
    public static bool isGameOver = false;
    public static bool isGamePaused = false;
    public static bool isGameLost = false;
    public static bool isGameWon = false;
    public static bool isAbilityUsageCountPanelShown = false;
    public static bool isAbilityInfoPanelShown = false;
    public static bool isWeaponInfoPanelShown = false;
    public static bool isAboutGamePanelShown = false;
    public TextMeshProUGUI timeRemainingText;
    public TextMeshProUGUI GameOverMessageText;
    public TextMeshProUGUI GameResultText;
    public TextMeshProUGUI MaximumUsageAbilityText;
    public TextMeshProUGUI GameResultDescriptionText;
    public TextMeshProUGUI GreedAbilityUsageCountText;
    public TextMeshProUGUI GluttonyAbilityUsageCountText;
    public TextMeshProUGUI EnvyAbilityUsageCountText;
    public TextMeshProUGUI WrathAbilityUsageCountText;
    public TextMeshProUGUI SlothAbilityUsageCountText;
    public CountDown countDown;
    private PlayerInventory playerInventory;
    private bool isGreedAbilityGranted = false;
    private bool isGluttonyAbilityGranted = false;
    private bool isEnvyAbilityGranted = false;
    private bool isSlothAbilityGranted = false;
    private bool isWrathAbilityGranted = false;
    private string playerTitle;

    private void Start()
    {
        Time.timeScale = 1;
        isGameOver = false;
        isGameLost = false;
        isGameWon = false;
        isGamePaused = false;
        isAbilityUsageCountPanelShown = false;
        isAbilityInfoPanelShown = false;
        isWeaponInfoPanelShown = false;
        isAboutGamePanelShown = false;
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        GameEndingPanel.SetActive(false);
        AbilityUsageCountPanel.SetActive(false);
        abilityInfoPanel.SetActive(false);
        weaponInfoPanel.SetActive(false);
        aboutGamePanel.SetActive(false);
        playerInventory = PlayerInventory.Instance;

        GameSoundManager.Instance.PlayBackgroundMusic();

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
        if (playerInventory != null && playerInventory.onGreedAbilityUsed != null)
        {
            playerInventory.onGreedAbilityUsed.AddListener(UpdateGreedAbilityUsedCount);
        }
        if (playerInventory != null && playerInventory.onGluttonyAbilityUsed != null)
        {
            playerInventory.onGluttonyAbilityUsed.AddListener(UpdateGluttonyAbilityUsedCount);
        }
        if (playerInventory != null && playerInventory.onEnvyAbilityUsed != null)
        {
            playerInventory.onEnvyAbilityUsed.AddListener(UpdateEnvyAbilityUsedCount);
        }
        if (playerInventory != null && playerInventory.onSlothAbilityUsed != null)
        {
            playerInventory.onSlothAbilityUsed.AddListener(UpdateSlothAbilityUsedCount);
        }
        if (playerInventory != null && playerInventory.onWrathAbilityUsed != null)
        {
            playerInventory.onWrathAbilityUsed.AddListener(UpdateWrathAbilityUsedCount);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGameOver && !isGameLost && !isAbilityUsageCountPanelShown && !isAbilityInfoPanelShown && !isWeaponInfoPanelShown && !isAboutGamePanelShown)
        {
            TogglePause();
        }

        if (Input.GetKey(KeyCode.Tab) && !isGamePaused && !isGameOver && !isGameLost && !isAbilityInfoPanelShown && !isWeaponInfoPanelShown && !isAboutGamePanelShown)
        {
            ShowAbilityUsageCount();
        }

        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            HideAbilityUsageCount();
        }
    }

    public void GameOver()
    {
        // audioSource.Stop();
        GameSoundManager.Instance.StopAllSounds();
        isGameOver = true;
        isGameLost = true;
        Time.timeScale = 0.1f;
        gameOverPanel.SetActive(true);
        string timeMessage = TimeToMessage(countDown.timeLeft);
        string gameOverMessage = RenderGameOverMessage();
        timeRemainingText.text = timeMessage.ToString();
        GameOverMessageText.text = gameOverMessage.ToString();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame has been called!");
        isGameOver = false;
        isGameLost = false;
        isGameWon = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private string TimeToMessage(float timeRemaining)
    {
        if (timeRemaining < 60)
        {
            return $"Just {timeRemaining:0} seconds away from victory!";
        }
        else
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            return $"Only {minutes} minutes left until victory!";
        }
    }

    private string RenderGameOverMessage()
    {
        string message = "\"Your courage will be remembered.\"\n" +
                         "\"This isn't the end. It's a new start.\"\n" +
                         "\"Challenge again to continue your legendary tale!\"\n";
        return message;
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            // audioSource.Pause();
            GameSoundManager.Instance.PauseAllSounds();
        }
        else
        {
            Time.timeScale = 1;
            AbilityUsageCountPanel.SetActive(false);
            pausePanel.SetActive(false);
            // audioSource.Play();
            GameSoundManager.Instance.UnPauseAllSounds();
        }
    }

    public void GameEnding()
    {
        // audioSource.Stop();
        GameSoundManager.Instance.StopAllSounds();
        isGameOver = true;
        Time.timeScale = 0;
        if (isGreedAbilityGranted && isGluttonyAbilityGranted && isEnvyAbilityGranted && isSlothAbilityGranted && isWrathAbilityGranted)
        {
            isGameWon = true;
            GameWin();
        }
        else
        {
            GameLose();
        }
    }

    public void ShowAbilityUsageCount()
    {
        isAbilityUsageCountPanelShown = true;
        AbilityUsageCountPanel.SetActive(true);
    }

    public void HideAbilityUsageCount()
    {
        isAbilityUsageCountPanelShown = false;
        AbilityUsageCountPanel.SetActive(false);
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

    public void ShowEpilogue()
    {
        if (!isGameWon)
        {
            LosingEpiloguePanel.SetActive(true);
        }
        else
        {
            WinningEpiloguePanel.SetActive(true);
        }
    }

    public void HideEpilogue()
    {
        if (!isGameWon)
        {
            LosingEpiloguePanel.SetActive(false);
        }
        else
        {
            WinningEpiloguePanel.SetActive(false);
        }
    }

    public void GameWin()
    {
        playerInventory.SetPlayerTitle();
        playerTitle = playerInventory.playerTitle;
        GameResultText.text = $"Congratulations!\n{playerTitle} Winner!";
        MaximumUsageAbilityText.text = $"({playerInventory.maximumUsageAbility})";
        GameResultDescriptionText.text = "\"You have successfully gathered all the rune fragments and their powers.\"\n" +
                                         "\"You emerged victorious in the subsequent battle against Karos Aldis.\"\n" +
                                         "\"Your bravery has protected Karpak, and your story will become legendary.\"\n" +
                                         "\"Cheers to your victory!\"";
        GameEndingPanel.SetActive(true);
    }

    public void GameLose()
    {
        playerInventory.SetPlayerTitle();
        playerTitle = playerInventory.playerTitle;
        GameResultText.text = $"Unfortunately!\n{playerTitle} Player!";
        MaximumUsageAbilityText.text = $"({playerInventory.maximumUsageAbility})";
        GameResultDescriptionText.text = "\"You failed to collect all the rune fragments and their power\"\n" +
                                         "\"You were devoured in the final battle against Karos Aldis\"\n" +
                                         "\"Though your quest ended in darkness\"\n" +
                                         "\"Your courage and perseverance have become a legend in the land of Karpak\"\n" +
                                         "\"Remember, every challenge is a necessary path to success\"\n" +
                                         "\"Challenge again, set forth anew\"\n" +
                                         "\"Until the legendary victory is yours!\"";
        GameEndingPanel.SetActive(true);
    }

    private void UpdateGreedAbility()
    {
        isGreedAbilityGranted = true;
    }

    private void UpdateGluttonyAbility()
    {
        isGluttonyAbilityGranted = true;
    }

    private void UpdateEnvyAbility()
    {
        isEnvyAbilityGranted = true;
    }

    private void UpdateSlothAbility()
    {
        isSlothAbilityGranted = true;
    }

    private void UpdateWrathAbility()
    {
        isWrathAbilityGranted = true;
    }

    private void UpdateGreedAbilityUsedCount()
    {
        GreedAbilityUsageCountText.text = $"Greed Ability:\t\t{playerInventory.greedAbilityUsageCount.ToString()} times";
    }

    private void UpdateGluttonyAbilityUsedCount()
    {
        GluttonyAbilityUsageCountText.text = $"Gluttony Ability:\t{playerInventory.gluttonyAbilityUsageCount.ToString()} times";
    }

    private void UpdateEnvyAbilityUsedCount()
    {
        EnvyAbilityUsageCountText.text = $"Envy Ability:\t\t{playerInventory.envyAbilityUsageCount.ToString()} times";
    }

    private void UpdateSlothAbilityUsedCount()
    {
        SlothAbilityUsageCountText.text = $"Sloth Ability:\t\t{playerInventory.slothAbilityUsageCount.ToString()} times";
    }

    private void UpdateWrathAbilityUsedCount()
    {
        WrathAbilityUsageCountText.text = $"Wrath Ability:\t\t{playerInventory.wrathAbilityUsageCount.ToString()} times";
    }
}