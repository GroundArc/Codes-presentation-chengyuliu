using UnityEngine;

public class GridItem : MonoBehaviour
{
    public Vector2 Id; // 格子 ID
    public GameObject content; // 存储格子上的物品
    public ItemType contentType = ItemType.None; // 存储格子物品的种类

    // 设置格子 ID
    public void SetId(Vector2 Id)
    {
        this.Id = Id;
    }

    // 放置物品到格子上
    public void PlaceContent(GameObject obj, ItemType type)
    {
        if (content != null)
        {
            Debug.LogWarning($"Grid ({Id.x}, {Id.y}) already contains an item of type {contentType}.");
            return;
        }

        content = obj;
        contentType = type;

        // 更新物品的位置信息
        if (content != null)
        {
            content.transform.position = GetTransPos();
        }
    }

    // 移除格子上的物品
    public void RemoveContent()
    {
        content = null;
        contentType = ItemType.None;
    }

    // 获取格子在世界坐标中的位置
    public Vector3 GetTransPos()
    {
        return this.transform.position + new Vector3(0, 1.2f, 0); // 偏移一定高度
    }
}

// 物品种类的枚举
public enum ItemType
{
    None,   // 空格子
    Player, // 玩家
    Monster, // 怪物
    Trap,   // 陷阱
    Chest   // 宝箱
}
