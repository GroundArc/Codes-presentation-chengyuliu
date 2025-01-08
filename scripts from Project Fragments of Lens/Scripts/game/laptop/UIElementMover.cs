using UnityEngine;
using System.Collections.Generic;

public class UIElementMover : MonoBehaviour
{
    public List<GameObject> uiElements = new List<GameObject>(); // 存储所有的 UI 元素
    public List<Vector3> uiPositions = new List<Vector3>();      // 存储所有的 UI 位置

    [Header("UI 提示框")]
    public GameObject addUIPrompt; // 提示 UI 添加的框
    public GameObject removeUIPrompt; // 提示 UI 删除的框
    public GameObject emptyListPrompt; // 当列表为空时显示的提示界面

    private void Start()
    {
        // 初始化时隐藏提示框
        addUIPrompt.SetActive(false);
        removeUIPrompt.SetActive(false);
        UpdateEmptyListPrompt();
    }

    // 添加一个新的 UI 元素，并将其移动到对应的队列位置
    public void AddUIElement(GameObject newUIElement)
    {
        if (uiElements.Count >= uiPositions.Count)
        {
            Debug.LogWarning("UI元素数量超过了位置数量，请检查位置列表。");
            return;
        }

        uiElements.Add(newUIElement);
        UpdateUIPositions();
        ShowAddUIPrompt();
        UpdateEmptyListPrompt(); // 检查空列表状态
    }

    // 删除一个 UI 元素，并让后续的元素向前移动
    public void RemoveUIElement(GameObject uiElementToRemove)
    {
        if (uiElements.Contains(uiElementToRemove))
        {
            uiElements.Remove(uiElementToRemove);
            UpdateUIPositions();
            ShowRemoveUIPrompt();
            UpdateEmptyListPrompt(); // 检查空列表状态
        }
        else
        {
            Debug.LogWarning("尝试删除的 UI 元素不存在于列表中。");
        }
    }

    // 更新所有 UI 元素的位置
    private void UpdateUIPositions()
    {
        for (int i = 0; i < uiElements.Count; i++)
        {
            // 确保位置和UI元素数量匹配
            if (i < uiPositions.Count)
            {
                uiElements[i].transform.localPosition = uiPositions[i];
            }
            else
            {
                Debug.LogWarning("位置数量不足，某些 UI 元素无法对齐到正确位置。");
            }
        }
    }

    // 显示添加 UI 的提示框，显示 1 秒后隐藏
    private void ShowAddUIPrompt()
    {
        addUIPrompt.SetActive(true);
    }


    // 显示删除 UI 的提示框，显示 1 秒后隐藏
    private void ShowRemoveUIPrompt()
    {
        removeUIPrompt.SetActive(true);
    }

    // 根据列表是否为空，更新空列表提示界面显示状态
    private void UpdateEmptyListPrompt()
    {
        emptyListPrompt.SetActive(uiElements.Count == 0);
    }
}
