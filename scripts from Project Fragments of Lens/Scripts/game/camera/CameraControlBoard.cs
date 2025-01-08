using UnityEngine;

public class CameraControlBoard : MonoBehaviour
{
    public Transform targetTransform; // Information board's camera transform
    private Vector3 initialPosition;
    private Vector3 initialEulerAngles;

    public float zoomSpeed = 0.5f;
    public float dragSpeed = 0.3f;
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 5f;

    private Vector3 currentCameraOffset;
    private bool isActive = false; // To track if the camera is focused on the board

    private void Start()
    {
        // Save initial transform data for resetting later
        initialPosition = transform.position;
        initialEulerAngles = transform.eulerAngles;

        // Calculate initial offset from target if needed
        if (targetTransform != null)
            currentCameraOffset = transform.position - targetTransform.position;
    }

    private void Update()
    {
        if (isActive)
        {
            HandleZoom();
            HandleDrag();
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentCameraOffset = Vector3.Lerp(currentCameraOffset, currentCameraOffset * (1 - scroll * zoomSpeed), Time.deltaTime * 5f);
        float currentDistance = currentCameraOffset.magnitude;
        currentCameraOffset = currentCameraOffset.normalized * Mathf.Clamp(currentDistance, minZoomDistance, maxZoomDistance);

        UpdateCameraPosition();
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButton(0))
        {
            float h = -Input.GetAxis("Mouse X") * dragSpeed;
            float v = -Input.GetAxis("Mouse Y") * dragSpeed;

            Vector3 dragOffset = new Vector3(h, v, 0);
            currentCameraOffset += Quaternion.Euler(0, -initialEulerAngles.y, 0) * dragOffset;

            UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {
        if (targetTransform != null)
            transform.position = targetTransform.position + currentCameraOffset;
    }

    public void ActivateBoardCamera(bool active)
    {
        isActive = active;
        if (active)
        {
            transform.position = targetTransform.position + currentCameraOffset;
            transform.eulerAngles = targetTransform.eulerAngles;
        }
        else
        {
            ResetCamera();
        }
    }

    private void ResetCamera()
    {
        transform.position = initialPosition;
        transform.eulerAngles = initialEulerAngles;
        currentCameraOffset = initialPosition - targetTransform.position;
    }
}
