using UnityEngine;

public class DestroyChildren : MonoBehaviour
{

    void Update()
    {
        // 检查是否按下了H键
        if (Input.GetKeyDown(KeyCode.H))
        {
            DestroyAllChildren();
        }
    }
    // 方法：摧毁所有子对象
    public void DestroyAllChildren()
    {
        // 遍历所有子对象并摧毁它们
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


}
