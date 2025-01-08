using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperBehaviour : MonoBehaviour
{
    public Wrapper wrapperData; // 关联的包装数据

    void Start()
    {
        if (wrapperData == null)
        {
            Debug.LogError("WrapperData not assigned to WrapperBehaviour on " + gameObject.name);
        }
    }

    // 可以在这里添加包装的特定行为和方法，例如：
    // - 检查包装是否在某个区域内
    // - 触发包装的特定动画或效果
    // - 管理包装的状态或属性

    // 示例方法：打印包装名称
    public void PrintWrapperName()
    {
        if (wrapperData != null)
        {
            Debug.Log("Wrapper name: " + wrapperData.wrapperName);
        }
    }
}
