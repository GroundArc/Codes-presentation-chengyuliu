using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Clickable3D : MonoBehaviour
{
    public LayerMask layerMask;
    public UnityEvent evt;
    public int mouseBtnNum = 0;
    public float dist = 30;

    private bool _check;
    public Outline outline;
    public bool isFlashing = false; // 控制闪烁开关
    public float flashFrequency = 1.0f; // 闪烁频率
    private float nextFlashTime = 0.0f;

    private Draggable3D _draggable3D;
    private int screenLayer;
    public bool interactionEnabled = true; // 新增开关，控制射线交互的启用或禁用


    private void Awake()
    {
        _draggable3D = GetComponent<Draggable3D>();
        screenLayer = LayerMask.NameToLayer("screen");
    }

    void LateUpdate()
    {
        // 如果交互被禁用，直接返回
        if (!interactionEnabled) return;

        bool mouseIsHover = false;

        // Create a ray from the mouse cursor on screen in the direction of the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, dist, layerMask))
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                mouseIsHover = true;
            }
        }

        // Handle flashing logic
        if (isFlashing && !mouseIsHover && Time.time >= nextFlashTime)
        {
            outline.enabled = !outline.enabled;
            nextFlashTime = Time.time + flashFrequency;
        }

        // Handle mouse hover
        if (mouseIsHover)
        {
            outline.enabled = true;  // Ensure outline is visible when hovering
            nextFlashTime = Time.time + flashFrequency; // Reset flash timer
        }
        else if (!isFlashing)
        {
            outline.enabled = false;  // Disable outline when not hovering and not flashing
        }

        // Handle click detection
        if (Input.GetMouseButtonDown(mouseBtnNum))
        {
            _check = mouseIsHover;
            if (_draggable3D != null && mouseIsHover)
            {
                _draggable3D.StartDrag();
            }
        }

        if (Input.GetMouseButtonUp(mouseBtnNum))
        {
            if (_draggable3D != null)
            {
                _draggable3D.EndDrag();
            }

            if (_check && mouseIsHover)
            {
                Debug.Log($"Clickable3D: Mouse clicked on object {gameObject.name}.");
                evt?.Invoke(); // Trigger the event
                _check = false;
            }
        }
    }

    // Call this to start flashing
    public void StartFlashing()
    {
        isFlashing = true;
    }

    // Call this to stop flashing
    public void StopFlashing()
    {
        isFlashing = false;
        outline.enabled = false; // Ensure the outline is disabled when stopping flashing
    }

    // 启用射线交互
    public void EnableInteraction()
    {
        interactionEnabled = true;
        Debug.Log("Interaction with clickable3D objects enabled.");
    }

    // 禁用射线交互
    public void DisableInteraction()
    {
        interactionEnabled = false;
        Debug.Log("Interaction with clickable3D objects disabled.");
    }

    // Check if the pointer is over a UI element in the "screen" layer
    private bool IsPointerOverScreenLayer()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            if (result.gameObject.layer == screenLayer)
            {
                return true;
            }
        }
        return false;
    }
}