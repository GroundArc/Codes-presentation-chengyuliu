using System.Collections.Generic;
using UnityEngine;

public class WrapperTray : MonoBehaviour
{
    public WrapperManager wrapperManager; // 包装管理器
    public Collider trayArea; // 托盘的特定区域（Collider 表示）
    public float spacing = 2.0f; // 包装之间的间距
    public float yOffset = 1.0f; // 包装在 Y 轴上的位置

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

    // 方法：显示包装
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
                GameObject wrapperInstance = Instantiate(wrapperPrefab, position, wrapperPrefab.transform.rotation); // 保持原有的旋转
                wrapperInstance.transform.SetParent(transform, true); // 使用 SetParent，保持世界坐标和缩放不变
                displayedWrappers.Add(wrapperInstance);
            }
            else
            {
                Debug.LogWarning("Wrapper prefab is missing for wrapper: " + availableWrappers[i].wrapperName);
            }
        }
    }

    // 方法：清除显示的包装
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
