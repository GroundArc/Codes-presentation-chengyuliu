using com;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera renderTextureCamera;
    public List<GameObject> objectsToHide; // List of objects to hide
    public UnityEvent onSwitchBackEvent; // 用于触发自定义事件
    public CaptureVideoFeedback capture;
    public Button exitButton;

    private Vector3 mainCameraOriginalPosition;
    private Quaternion mainCameraOriginalRotation;
    private bool isSwitched = false;


    void Start()
    {
        // Store the initial position and rotation of the main camera
        mainCameraOriginalPosition = mainCamera.transform.position;
        mainCameraOriginalRotation = mainCamera.transform.rotation;

        // Add a listener to the exitButton for the same effect as pressing Escape or right mouse button
        exitButton.onClick.AddListener(SwitchBackToMainCamera);
    }

    // Method to switch the camera view to the Render Texture Camera
    public void SwitchToRenderTextureCamera()
    {
        mainCamera.transform.position = renderTextureCamera.transform.position;
        mainCamera.transform.rotation = renderTextureCamera.transform.rotation;
        // Disable the Render Texture Camera to avoid dual camera rendering issues
        renderTextureCamera.enabled = false;
        isSwitched = true;
        capture.SetPostProcessingVolume_Video();
    }

    void Update()
    {
        // Check if the Escape key is pressed or right mouse button is clicked
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && isSwitched)
        {
            SwitchBackToMainCamera();
        }
    }

    // Method to switch back to the main camera and hide objects
    private void SwitchBackToMainCamera()
    {
        // Return the main camera to its original position and rotation
        mainCamera.transform.position = mainCameraOriginalPosition;
        mainCamera.transform.rotation = mainCameraOriginalRotation;
        renderTextureCamera.enabled = true;
        isSwitched = false;

        // Hide specified objects
        HideObjects();

        // Trigger custom event and reset post-processing volume
        onSwitchBackEvent.Invoke();
        capture.SetPostProcessingVolume_Normal();
    }

    public bool GetSwitched() 
    {
        return isSwitched;
    }
    // Method to hide specified objects
    private void HideObjects()
    {
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning("One of the objects to hide is not set.");
            }
        }
    }
}
