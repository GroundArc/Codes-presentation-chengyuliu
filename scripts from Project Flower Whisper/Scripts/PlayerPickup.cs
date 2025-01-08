using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform player; // ��Ҷ���
    public Transform containerHoldPosition; // Container��Ϊ����Ӷ���������

    private GameObject currentContainer; // ��ǰ�����ڵ�Container
    private bool isPlayerInArea = false; // ��־λ����ʾ����Ƿ���������

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
            // ��Container�����ҵ��Ӷ���
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
