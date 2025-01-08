using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ContainerManager : MonoBehaviour
{
    public Transform containerPrefab; // Container的预制体
    public Transform targetArea; // 目标区域的Transform
    public Canvas floatingCanvas; // 漂浮文字的Canvas
    public TMP_Text warningText; // 提示文本的TextPro区域
    public Camera mainCamera; // 主摄像机
    public Button copyButton; // 用于触发复制操作的按钮
    public PlayerPickup playerPickup; // 引用PlayerPickup脚本

    private GameObject currentContainer; // 当前存在的Container

    private void Start()
    {
        if (copyButton != null)
        {
            copyButton.onClick.AddListener(CheckAndCopyContainer);
        }

        floatingCanvas.gameObject.SetActive(false); // 初始时隐藏漂浮文字
        warningText.gameObject.SetActive(false); // 初始时隐藏警告文字
    }

    private void Update()
    {
        // 让漂浮文字始终面朝摄像机方向
        if (floatingCanvas.gameObject.activeSelf)
        {
            floatingCanvas.transform.LookAt(mainCamera.transform);
            floatingCanvas.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
    }

    private void CheckAndCopyContainer()
    {
        if (containerPrefab == null)
        {
            Debug.LogError("ContainerPrefab is not assigned.");
            return;
        }

        // 检查Container的子对象Tag
        int wrapperCount = 0;
        int accessoryCount = 0;
        int flowerCount = 0;

        foreach (Transform child in containerPrefab)
        {
            if (child.CompareTag("Wrapper"))
            {
                wrapperCount++;
            }
            else if (child.CompareTag("Accessory"))
            {
                accessoryCount++;
            }
            else if (child.CompareTag("Flower"))
            {
                flowerCount++;
            }
        }

        List<string> warnings = new List<string>();

        if (wrapperCount > 1)
        {
            warnings.Add("Container can only contain one Wrapper.");
        }

        if (accessoryCount > 1)
        {
            warnings.Add("Container can only contain one Accessory.");
        }

        if (flowerCount > 3)
        {
            warnings.Add("Container can only contain up to three Flowers.");
        }

        if (warnings.Count > 0)
        {
            // 在指定TextPro区域生成提示
            warningText.text = string.Join("\n", warnings);
            ShowWarningText();
        }
        else
        {
            CopyContainer();
        }
    }

    private void ShowWarningText()
    {
        // 显示警告文字
        warningText.gameObject.SetActive(true);

        // 使用DoTween进行震动效果
        warningText.transform.DOShakePosition(1f, 10, 10, 90, false, true);

        // 2秒后隐藏警告文字
        DOVirtual.DelayedCall(2f, () =>
        {
            warningText.gameObject.SetActive(false);
        });
    }

    private void CopyContainer()
    {
        if (targetArea == null)
        {
            Debug.LogError("TargetArea is not assigned.");
            return;
        }

        // 检查containerPrefab中是否有任何子对象
        if (containerPrefab.childCount == 0)
        {
            Debug.LogWarning("No child objects in the container to copy.");
            return;
        }

        // 确保目标区域只存在一个Container
        if (currentContainer != null)
        {
            Destroy(currentContainer);
        }

        // 生成新的Container对象
        currentContainer = Instantiate(containerPrefab.gameObject, targetArea.position, Quaternion.Euler(-90, 0, 0));
        currentContainer.name = containerPrefab.name + "_Copy";

        // 移除 FlowerLanguageDisplay 组件
        FlowerLanguageDisplay languageDisplay = currentContainer.GetComponent<FlowerLanguageDisplay>();
        if (languageDisplay != null)
        {
            Destroy(languageDisplay);
        }

        // 更新PlayerPickup中的currentContainer
        playerPickup.UpdateCurrentContainer(currentContainer);

        // 显示漂浮文字
        floatingCanvas.gameObject.SetActive(true);
       

        // 清空原Container的子对象
        ClearContainerChildren(containerPrefab);
    }

    private void ClearContainerChildren(Transform containerTransform)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in containerTransform)
        {
            // 仅将未标记为 "UnDelete" 的子对象添加到待删除列表中
            if (!child.CompareTag("UnDelete"))
            {
                children.Add(child);
            }
        }

        // 删除所有已收集到的子对象
        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }
    }

}
