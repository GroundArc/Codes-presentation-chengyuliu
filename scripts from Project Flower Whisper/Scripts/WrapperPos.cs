using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperPos : MonoBehaviour
{
    public Collider containerCollider; // Container 的碰撞器
    public Transform tray; // WrapperTray 对象
    public Transform container; // Container 对象

    private WrapperManager wrapperManager;
    private WrapperBehaviour wrapperBehaviour;
    private Vector3 initialPosition;
    private Transform initialParent;
    private Vector3 originalScale; // 原始缩放比例
    private bool isInTray = false; // 标志位，表示是否在托盘区域内

    private WrapperTray wrapperTray;

    void Start()
    {
        wrapperManager = FindObjectOfType<WrapperManager>();
        if (wrapperManager == null)
        {
            Debug.LogError("WrapperManager not found in the scene.");
            return;
        }

        wrapperBehaviour = GetComponent<WrapperBehaviour>();
        if (wrapperBehaviour == null || wrapperBehaviour.wrapperData == null)
        {
            Debug.LogError("WrapperBehaviour or wrapperData not found on " + gameObject.name);
            return;
        }

        if (tray == null)
        {
            tray = GameObject.FindGameObjectWithTag("WrapperTray")?.transform;
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

        wrapperTray = tray.GetComponent<WrapperTray>();
        if (wrapperTray == null)
        {
            Debug.LogError("WrapperTray script not found on " + tray.name);
            return;
        }

        Debug.Log("Tray found: " + (tray != null));
        Debug.Log("Container found: " + (container != null));

        // 初始化位置、父对象和缩放比例
        initialPosition = transform.position;
        initialParent = transform.parent;
        originalScale = transform.localScale;

        // 初始检查包装位置
        CheckWrapperPosition();
    }

    void Update()
    {
        CheckWrapperPosition();
    }

    private void CheckWrapperPosition()
    {
        Vector3 wrapperPosition = transform.position;

        if (wrapperTray.IsInTrayXZBounds(wrapperPosition))
        {
            // 在托盘区域内
            if (!isInTray || transform.parent != tray)
            {
                isInTray = true;
                SetParentWithReset(transform, tray, originalScale);

                wrapperManager.Add(wrapperBehaviour.wrapperData);
                Debug.Log("Wrapper in tray, added to WrapperManager: " + wrapperBehaviour.wrapperData.wrapperName);

            }
        }
        else
        {
            // 在容器区域内或不在任何区域内
            if (isInTray || transform.parent != container)
            {
                isInTray = false;
                SetParentWithReset(transform, container, Vector3.one);

                wrapperManager.Remove(wrapperBehaviour.wrapperData);
                Debug.Log("Wrapper in container, removed from WrapperManager: " + wrapperBehaviour.wrapperData.wrapperName);

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
