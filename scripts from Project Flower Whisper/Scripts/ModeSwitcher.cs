using UnityEngine;
using Cinemachine;

public class ModeSwitcher : MonoBehaviour
{
    public CinemachineFreeLook mainFreeLookCamera;
    public CinemachineVirtualCamera flowerEditorVirtualCamera;
    public CinemachineVirtualCamera eventOneCamera;
    public GameObject mainCanvas;
    public GameObject flowerEditorCanvas;
    public FlowerTray flowerTray; // 添加FlowerTray引用
    

    [HideInInspector]
    public bool isInFlowerEditorMode = false;

    void Start()
    {
        SetMainMode();
    }

    void Update()
    {
        if (isInFlowerEditorMode && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitFlowerEditorMode();
        }
    }

    public void EnterFlowerEditorMode()
    {
        isInFlowerEditorMode = true;
        SetFlowerEditorMode();
    }

    public void ExitFlowerEditorMode()
    {
        isInFlowerEditorMode = false;
        SetMainMode();
    }

    public void SetMainMode()
    {
        if (mainFreeLookCamera != null)
        {
            mainFreeLookCamera.Priority = 10;
            Debug.Log("Main FreeLook camera priority set to 10.");
        }
        else
        {
            Debug.LogError("Main FreeLook camera is not assigned.");
        }

        if (flowerEditorVirtualCamera != null)
        {
            flowerEditorVirtualCamera.Priority = 0;
            Debug.Log("Flower editor virtual camera priority set to 0.");
        }

        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
            Debug.Log("Main canvas enabled.");
        }

        if (flowerEditorCanvas != null)
        {
            flowerEditorCanvas.SetActive(false);
            Debug.Log("Flower editor canvas disabled.");
        }
    }
    public void SetEventCameraMode()
    {
        if (mainFreeLookCamera != null)
        {
            mainFreeLookCamera.Priority = 0;
            Debug.Log("Main FreeLook camera priority set to 0.");
        }

        if (eventOneCamera != null)
        {
            eventOneCamera.Priority = 10;
            Debug.Log("event camera priority set to 10.");
        }
        else
        {
            Debug.LogError("event virtual camera is not assigned.");
        }

        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
            Debug.Log("Main canvas disabled.");
        }

        if (flowerEditorCanvas != null)
        {
            flowerEditorCanvas.SetActive(false );
            Debug.Log("Flower editor canvas disabled.");
        }
    }

    public void SetFlowerEditorMode()
    {
        if (mainFreeLookCamera != null)
        {
            mainFreeLookCamera.Priority = 0;
            Debug.Log("Main FreeLook camera priority set to 0.");
        }

        if (flowerEditorVirtualCamera != null)
        {
            flowerEditorVirtualCamera.Priority = 10;
            Debug.Log("Flower editor virtual camera priority set to 10.");
        }
        else
        {
            Debug.LogError("Flower editor virtual camera is not assigned.");
        }

        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
            Debug.Log("Main canvas disabled.");
        }

        if (flowerEditorCanvas != null)
        {
            flowerEditorCanvas.SetActive(true);
            Debug.Log("Flower editor canvas enabled.");
        }

        // 更新托盘
        if (flowerTray != null)
        {
            flowerTray.DisplayFlowers();
            Debug.Log("Flower tray updated.");
        }
        else
        {
            Debug.LogError("FlowerTray is not assigned.");
        }

       
    }
}