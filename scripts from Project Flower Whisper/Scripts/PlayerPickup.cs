using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform player; // 玩家对象
    public Transform containerHoldPosition; // Container成为玩家子对象后的坐标

    private GameObject currentContainer; // 当前区域内的Container
    private bool isPlayerInArea = false; // 标志位，表示玩家是否在区域内

    private void Update()
    {
        if (isPlayerInArea && Input.GetKeyDown(KeyCode.F))
        {
            PickupContainer();
        }
    }

    public void UpdateCurrentContainer(GameObject newContainer)
    {
        currentContainer = newContainer;
    }

    private void PickupContainer()
    {
        if (currentContainer != null)
        {
            // 将Container变成玩家的子对象
            currentContainer.transform.SetParent(containerHoldPosition);
            currentContainer.transform.localPosition = Vector3.zero;
            currentContainer.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            Debug.Log("Picked up container: " + currentContainer.name);
        }
        else
        {
            Debug.Log("No container to pick up.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = true;
            Debug.Log("Player entered pickup range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
            Debug.Log("Player exited pickup range.");
        }
    }
}
