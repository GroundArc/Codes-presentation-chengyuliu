using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeZoomCamera : MonoBehaviour
{
    public Transform targetCamera;
    public Transform cameraTransRef;
    public float distMax;
    public float speed;
    float _dist;
    Vector3 _zoomPoint;
    public LayerMask layerMask;
    public float dist = 30;
    public GameObject board;
    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y == 1)
        {
            ZoomIn();
        }
        else if (Input.mouseScrollDelta.y == -1)
        {
            ZoomOut();
        }
    }

    public void Init()
    {
        _dist = 0;
        enabled = true;
    }

    void ZoomIn()
    {
        if (_dist >= distMax)
            return;

        _dist += speed;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, dist, layerMask))
        {
            if (hit.transform.gameObject == board)
            {
                _zoomPoint = hit.point;
            }
        }
        SyncZoom();
    }

    void ZoomOut()
    {
        if (_dist <= 0)
            return;

        _dist -= speed;
        SyncZoom();
    }

    void SyncZoom()
    {
        _dist = Mathf.Clamp(_dist, 0, distMax);
        var dir = _zoomPoint - cameraTransRef.position;
        dir.Normalize();
        targetCamera.position = cameraTransRef.position + dir * _dist;
    }
}