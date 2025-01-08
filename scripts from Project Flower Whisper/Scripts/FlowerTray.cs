using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FlowerTray : MonoBehaviour
{
    public FlowerManager flowerManager; // ���������
    public Collider trayArea; // ���̵��ض�����Collider ��ʾ��
    public float spacing = 2.0f; // ����֮��ļ��
    public float yOffset = 1.0f; // ������ Y ���ϵ�λ��
    public Transform container;
    public List<GameObject> displayedFlowers = new List<GameObject>();
    

    void Start()
    {
        if (flowerManager == null)
        {
            //Debug.LogError("FlowerManager is not assigned.");
            return;
        }

        if (trayArea == null)
        {
            //Debug.LogError("Tray area is not assigned.");
            return;
        }

        DisplayFlowers();
    }

    // ��������ʾ����
    public void DisplayFlowers()
    {
        ClearDisplayedFlowers();

        List<Flower> availableFlowers = flowerManager.flowers;
        if (availableFlowers == null || availableFlowers.Count == 0)
        {
            Debug.LogWarning("No available flowers to display.");
            return;
        }

        Vector3 trayCenter = trayArea.bounds.center;
        float startX = trayCenter.x - (availableFlowers.Count - 1) * spacing / 2.0f;
        float yPosition = trayCenter.y + yOffset;
        float zPosition = trayCenter.z;

        for (int i = 0; i < availableFlowers.Count; i++)
        {
            GameObject flowerPrefab = availableFlowers[i].flowerPrefab;
            if (flowerPrefab != null)
            {
                Vector3 position = new Vector3(startX + i * spacing, yPosition, zPosition);
                GameObject flowerInstance = Instantiate(flowerPrefab, position, flowerPrefab.transform.rotation); // ����ԭ�е���ת
                flowerInstance.transform.SetParent(transform, true); // ʹ�� SetParent������������������Ų���
                displayedFlowers.Add(flowerInstance);
            }
            else
            {
                Debug.LogWarning("Flower prefab is missing for flower: " + availableFlowers[i].flowerName);
            }
        }
    }

    // �����������ʾ�Ļ���
    public void ClearDisplayedFlowers()
    {
        for (int i = displayedFlowers.Count - 1; i >= 0; i--)
        {
            if (displayedFlowers[i] != null && displayedFlowers[i].transform.parent != container)
            {
                Destroy(displayedFlowers[i]);
                displayedFlowers.RemoveAt(i);
            }
        }
    }

    public bool IsInTrayXZBounds(Vector3 position)
    {
        Bounds bounds = trayArea.bounds;
        bool isInXBounds = position.x >= bounds.min.x && position.x <= bounds.max.x;
        bool isInZBounds = position.z >= bounds.min.z && position.z <= bounds.max.z;

        bool isInBounds = isInXBounds && isInZBounds;

        //Debug.Log("Position " + position + " is within tray X bounds: " + isInXBounds + " and Z bounds: " + isInZBounds);
        return isInBounds;
    }
}
