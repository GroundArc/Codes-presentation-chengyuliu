using System.Collections.Generic;
using UnityEngine;

public class WrapperTray : MonoBehaviour
{
    public WrapperManager wrapperManager; // ��װ������
    public Collider trayArea; // ���̵��ض�����Collider ��ʾ��
    public float spacing = 2.0f; // ��װ֮��ļ��
    public float yOffset = 1.0f; // ��װ�� Y ���ϵ�λ��

    private List<GameObject> displayedWrappers = new List<GameObject>();

    void Start()
    {
        if (wrapperManager == null)
        {
            //Debug.LogError("WrapperManager is not assigned.");
            return;
        }

        if (trayArea == null)
        {
            //Debug.LogError("Tray area is not assigned.");
            return;
        }

        DisplayWrappers();
    }

    // ��������ʾ��װ
    public void DisplayWrappers()
    {
        ClearDisplayedWrappers();

        List<Wrapper> availableWrappers = wrapperManager.wrappers;
        if (availableWrappers == null || availableWrappers.Count == 0)
        {
            Debug.LogWarning("No available wrappers to display.");
            return;
        }

        Vector3 trayCenter = trayArea.bounds.center;
        float startX = trayCenter.x - (availableWrappers.Count - 1) * spacing / 2.0f;
        float yPosition = trayCenter.y + yOffset;
        float zPosition = trayCenter.z;

        for (int i = 0; i < availableWrappers.Count; i++)
        {
            GameObject wrapperPrefab = availableWrappers[i].wrapperPrefab;
            if (wrapperPrefab != null)
            {
                Vector3 position = new Vector3(startX + i * spacing, yPosition, zPosition);
                GameObject wrapperInstance = Instantiate(wrapperPrefab, position, wrapperPrefab.transform.rotation); // ����ԭ�е���ת
                wrapperInstance.transform.SetParent(transform, true); // ʹ�� SetParent������������������Ų���
                displayedWrappers.Add(wrapperInstance);
            }
            else
            {
                Debug.LogWarning("Wrapper prefab is missing for wrapper: " + availableWrappers[i].wrapperName);
            }
        }
    }

    // �����������ʾ�İ�װ
    public void ClearDisplayedWrappers()
    {
        foreach (GameObject wrapper in displayedWrappers)
        {
            Destroy(wrapper);
        }
        displayedWrappers.Clear();
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
