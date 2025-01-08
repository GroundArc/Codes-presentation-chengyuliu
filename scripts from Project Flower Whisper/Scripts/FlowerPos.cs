using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPos : MonoBehaviour
{
    public Collider containerCollider; // Container 的碰撞器
    public Transform tray; // FlowerTray 对象
    public Transform container; // Container 对象

    private FlowerManager flowerManager;
    private FlowerBehaviour flowerBehaviour;
    private Vector3 initialPosition;
    private Transform initialParent;
    private Vector3 originalScale; // 原始缩放比例
    private bool isInTray = false; // 标志位，表示是否在托盘区域内
    private bool hasLeftTray = false; // 标志位，表示是否已经离开托盘区域

    private FlowerTray flowerTray;

    void Start()
    {
        flowerManager = FindObjectOfType<FlowerManager>();
        if (flowerManager == null)
        {
            Debug.LogError("FlowerManager not found in the scene.");
            return;
        }

        flowerBehaviour = GetComponent<FlowerBehaviour>();
        if (flowerBehaviour == null || flowerBehaviour.flowerData == null)
        {
            Debug.LogError("FlowerBehaviour or flowerData not found on " + gameObject.name);
            return;
        }

        if (tray == null)
        {
            tray = GameObject.FindGameObjectWithTag("FlowerTray")?.transform;
        }
        if (container == null)
        {
            container = GameObject.FindGameObjectWithTag("Container")?.transform;
        }

        if (tray == null || container == null)
        {
            Debug.LogError("Tray or Container not found.");
            return;
        }

        flowerTray = tray.GetComponent<FlowerTray>();
        if (flowerTray == null)
        {
            Debug.LogError("FlowerTray script not found on " + tray.name);
            return;
        }

        Debug.Log("Tray found: " + (tray != null));
        Debug.Log("Container found: " + (container != null));

        // 初始化位置、父对象和缩放比例
        initialPosition = transform.position;
        initialParent = transform.parent;
        originalScale = transform.localScale;

        // 初始检查花朵位置
        CheckFlowerPosition();
    }

    void Update()
    {
        CheckFlowerPosition();
    }

    private void CheckFlowerPosition()
    {
        Vector3 flowerPosition = transform.position;

        if (flowerTray.IsInTrayXZBounds(flowerPosition))
        {
            // 在托盘区域内
            if (!isInTray || transform.parent != tray)
            {
                isInTray = true;
                SetParentWithReset(transform, tray, originalScale);

                // 只有当花已经离开过托盘区域时，才允许重新添加到FlowerManager
                if (hasLeftTray && !flowerManager.flowers.Contains(flowerBehaviour.flowerData))
                {
                    flowerManager.Add(flowerBehaviour.flowerData);
                    Debug.Log("Flower in tray, added to FlowerManager: " + flowerBehaviour.flowerData.flowerName);
                }
            }
        }
        else
        {
            // 在容器区域内或不在任何区域内
            if (isInTray || transform.parent != container)
            {
                isInTray = false;
                SetParentWithReset(transform, container, Vector3.one);

                if (flowerManager.flowers.Contains(flowerBehaviour.flowerData))
                {
                    flowerManager.Remove(flowerBehaviour.flowerData);
                    hasLeftTray = true; // 标记花朵已经离开过托盘区域
                    Debug.Log("Flower in container, removed from FlowerManager: " + flowerBehaviour.flowerData.flowerName);
                }
            }
        }
    }

    private void SetParentWithReset(Transform child, Transform parent, Vector3 scale)
    {
        // 获取当前全局位置
        Vector3 globalPosition = child.position;
        Quaternion globalRotation = child.rotation;

        // 设置新的父对象
        child.SetParent(parent);

        // 重新设置全局位置和旋转
        child.position = globalPosition;
        child.localScale = scale;
        child.rotation = globalRotation;

        Debug.Log("Set parent to " + parent.name);
    }
}
