using System.Collections.Generic;
using UnityEngine;

public class PigmentTray : MonoBehaviour
{
    public ColorOwned colorOwned; // 颜色管理器
    public Collider trayArea; // 用于定义托盘区域的碰撞器
    public int columns = 3; // 每行的颜料块数量

    private List<GameObject> displayedPigments = new List<GameObject>();

    void Start()
    {
        if (colorOwned == null)
        {
            Debug.LogError("ColorOwned is not assigned.");
            return;
        }

        if (trayArea == null)
        {
            Debug.LogError("Tray area is not assigned.");
            return;
        }

        DisplayPigments();
        colorOwned.onItemChangedCallBack += DisplayPigments; // 订阅物品变化事件
    }

    void OnDestroy()
    {
        colorOwned.onItemChangedCallBack -= DisplayPigments; // 取消订阅物品变化事件
    }

    // 方法：显示颜料块
    public void DisplayPigments()
    {
        ClearDisplayedPigments();

        List<ItemStack> items = colorOwned.GetItems();
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("No available pigments to display.");
            return;
        }

        Bounds bounds = trayArea.bounds;
        float spacingX = bounds.size.x / columns;
        int rows = Mathf.CeilToInt(items.Count / (float)columns);
        float spacingZ = bounds.size.z / rows;
        float yPosition = bounds.center.y; // 使用碰撞器的中心Y值

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].quantity > 0 && items[i].item.pigmentPrefab != null)
            {
                int row = i / columns;
                int col = i % columns;

                Vector3 position = new Vector3(
                    bounds.min.x + col * spacingX + spacingX / 2,
                    yPosition,
                    bounds.min.z + row * spacingZ + spacingZ / 2
                );

                GameObject pigmentInstance = Instantiate(items[i].item.pigmentPrefab, position, Quaternion.identity);
                pigmentInstance.transform.SetParent(trayArea.transform, true);
                pigmentInstance.transform.localPosition = position - trayArea.transform.position; // 确保位置正确
                pigmentInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 设置缩放为0.5
                pigmentInstance.GetComponent<PigmentDisplay>().SetQuantity(items[i].quantity);
                pigmentInstance.GetComponent<DragPigment>().initialPosition = pigmentInstance.transform.position; // 设置初始位置
                pigmentInstance.GetComponent<DragPigment>().initialParent = trayArea.transform; // 设置初始父对象
                pigmentInstance.GetComponent<DragPigment>().pigmentItem = items[i].item; // 设置颜料块对应的Item
                displayedPigments.Add(pigmentInstance);
            }
        }
    }

    // 方法：清除显示的颜料块
    public void ClearDisplayedPigments()
    {
        foreach (GameObject pigment in displayedPigments)
        {
            Destroy(pigment);
        }
        displayedPigments.Clear();
    }
}
