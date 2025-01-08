using UnityEngine;

public class EazyCameraControlBoard : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float dragSpeed = 0.3f;
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 5f;

    private Vector3 currentCameraOffset;
    private Camera mainCamera;

    private void Start()
    {
        // 设置相机初始位置为最远缩放位置
        currentCameraOffset = transform.position.normalized * maxZoomDistance;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleZoom();
        HandleDrag();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 direction = mainCamera.ScreenPointToRay(Input.mousePosition).direction;
            currentCameraOffset += direction * scroll * zoomSpeed;

            float currentDistance = currentCameraOffset.magnitude;
            currentCameraOffset = currentCameraOffset.normalized * Mathf.Clamp(currentDistance, minZoomDistance, maxZoomDistance);

            UpdateCameraPosition();
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 dragOffset = transform.position - hitPoint;

                float h = -Input.GetAxis("Mouse X") * dragSpeed;
                float v = -Input.GetAxis("Mouse Y") * dragSpeed;

                Vector3 moveDirection = new Vector3(h, v, 0);
                currentCameraOffset += Quaternion.Euler(0, -transform.eulerAngles.y, 0) * moveDirection;

                UpdateCameraPosition();
            }
        }
    }

    private void UpdateCameraPosition()
    {
        transform.position = currentCameraOffset;
    }
}