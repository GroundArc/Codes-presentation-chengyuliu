using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTriggerController : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    private CameraController cameraController;
    private bool playerInside = false;

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController == null)
        {
            Debug.LogError("No CameraController found on the main camera.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.isGameOver)
        {
            return;
        }

        if (other.CompareTag("Player") && !playerInside)
        {
            playerInside = true;
            Debug.Log("Player entered the room.");

            if (spawner != null)
            {
                spawner.StartSpawning();
            }
            else
            {
                Debug.LogError("Spawner reference is not set in RoomTriggerController.");
            }

            SetCameraBounds();
        }
    }

    private void SetCameraBounds()
    {
        Collider roomCollider = this.GetComponent<Collider>();
        if (roomCollider != null && cameraController != null)
        {
            Vector3 minBounds = roomCollider.bounds.min;
            Vector3 maxBounds = roomCollider.bounds.max;
            float minXBound = minBounds.x + 3.0f;
            float maxXBound = maxBounds.x - 3.0f;
            cameraController.SetBound(minBounds.z, minXBound, maxXBound);
        }
        else
        {
            Debug.LogError("No collider attached to the room or CameraController not found.");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.StopSpawning();
            playerInside = false;
            Debug.Log("Player left the room.");
        }
    }

}
