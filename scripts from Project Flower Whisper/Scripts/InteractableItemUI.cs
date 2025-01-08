using UnityEngine;
using TMPro;

public class InteractableItemUI : MonoBehaviour
{
    
    public float displayRange = 3f;
    public GameObject floatingText; // Assign this in the inspector
    public bool faceCamera = true; // Option to make the text face the camera
    public Outline outline; // Reference to the Outline component

    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        if (floatingText != null)
        {
            
            floatingText.SetActive(false);
        }

        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Find the main camera
        mainCamera = Camera.main;

        // Ensure the outline is initially disabled
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void Update()
    {
        if (player != null && floatingText != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= displayRange)
            {
                floatingText.SetActive(true);
                if (outline != null)
                {
                    outline.enabled = true; // Enable outline when in range
                }
                if (faceCamera && mainCamera != null)
                {
                    floatingText.transform.LookAt(mainCamera.transform);
                    floatingText.transform.Rotate(0, 180, 0); // Correct the text orientation
                }
            }
            else
            {
                floatingText.SetActive(false);
                if (outline != null)
                {
                    outline.enabled = false; // Disable outline when out of range
                }
            }
        }
    }
}
