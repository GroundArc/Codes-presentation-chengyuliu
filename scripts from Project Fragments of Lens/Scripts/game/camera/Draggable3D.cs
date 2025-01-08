using Assets.Scripts.game.InformationBoard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable3D : MonoBehaviour
{
    public LayerMask layerMask;
    public float dist = 30f; // Raycast的检测距离
    public bool isDragging { get; private set; }
    private GameObject board;

    public Vector3 offset;

    private void Start()
    {
        // 获取 InformationBoardSystem 实例并获取 board
        board = InformationBoardSystem.instance.GetBoardPlane();
        if (board == null)
        {
            Debug.LogError("Board plane is not set in InformationBoardSystem.");
        }
        else
        {
            Debug.Log("Successfully retrieved the board from InformationBoardSystem.");
        }
    }

    public void StartDrag()
    {
        isDragging = true;
        Debug.Log($"Start dragging {gameObject.name}");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        offset = Vector3.zero;
        // Perform the raycast
        if (Physics.Raycast(ray, out hit, dist, layerMask))
        {
            Debug.Log($"Raycast hit {hit.transform.gameObject.name}");
            if (hit.transform.gameObject == board)
            {
                offset = transform.position - hit.point;
                Debug.Log($"Offset calculated: {offset}");
            }
            else
            {
                Debug.LogWarning("Raycast hit an object other than the board.");
            }
        }
        else
        {
            Debug.LogWarning("Raycast did not hit anything.");
        }
    }

    public void EndDrag()
    {
        isDragging = false;
        Debug.Log($"End dragging {gameObject.name}");
    }

    private void Update()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, dist, layerMask))
            {
                Debug.Log($"Dragging: Raycast hit {hit.transform.gameObject.name}");
                if (hit.transform.gameObject == board)
                {
                    transform.position = hit.point + offset;
                    Debug.Log($"Updated object position: {transform.position}");
                }
                else
                {
                    Debug.LogWarning("Raycast hit an object other than the board while dragging.");
                }
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything while dragging.");
            }
        }
    }
}
