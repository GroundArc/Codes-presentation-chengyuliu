using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject notification;
    public GameObject colorDerive;
    public static bool GameisPause = false;
    public CharacterController playerMovement; // 引用玩家移动脚本
    public GameObject colorD;
        
    

   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (GameisPause)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }

        
    }

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        notification.SetActive(true);
        colorDerive.SetActive(true);
        Time.timeScale = 1f;
        GameisPause = false;
        playerMovement.enabled = true;
        colorD.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        notification.SetActive(false);
        colorDerive.SetActive(false);
        Time.timeScale = 0f;
        GameisPause = true;
        playerMovement.enabled = false;
        colorD.SetActive(false);
    }

    public void LoadMenu() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    

    
}
