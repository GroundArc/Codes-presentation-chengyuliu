using UnityEngine;
using System.Collections.Generic;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject emptyMessagePanel;      // 提示框，当列表为空时显示
    public List<GameObject> panels = new List<GameObject>(); // 可动态添加和删除的面板列表

    private int currentPanelIndex = -1; // 当前显示的面板索引，-1表示无面板显示

    // 初始化时检测列表是否为空
    void Start()
    {
        UpdateEmptyMessagePanel();
        ShowCurrentPanel();
    }

    // 显示下一个面板
    public void ShowNextPanel()
    {
        if (panels.Count == 0) return; // 列表为空则不操作

        currentPanelIndex = (currentPanelIndex + 1) % panels.Count; // 循环到下一个索引
        ShowCurrentPanel();
    }

    // 显示上一个面板
    public void ShowPreviousPanel()
    {
        if (panels.Count == 0) return; // 列表为空则不操作

        currentPanelIndex = (currentPanelIndex - 1 + panels.Count) % panels.Count; // 循环到上一个索引
        ShowCurrentPanel();
    }

    // 添加新的面板
    public void AddPanel(GameObject newPanel)
    {
        panels.Add(newPanel); // 添加面板
        UpdateEmptyMessagePanel(); // 检测是否显示提示框
        if (currentPanelIndex == -1) currentPanelIndex = 0; // 设置为第一个面板
        ShowCurrentPanel();
    }

    // 删除面板
    public void RemovePanel(GameObject panelToRemove)
    {
        if (panels.Contains(panelToRemove))
        {
            panels.Remove(panelToRemove); // 移除面板
            UpdateEmptyMessagePanel(); // 检测是否显示提示框

            // 调整当前显示的面板索引
            if (currentPanelIndex >= panels.Count)
            {
                currentPanelIndex = panels.Count - 1;
            }
            ShowCurrentPanel();
        }
    }

    // 更新提示框的显示状态
    private void UpdateEmptyMessagePanel()
    {
        if (emptyMessagePanel != null)
        {
            emptyMessagePanel.SetActive(panels.Count == 0); // 当列表为空时显示提示框
        }
    }

    // 显示当前索引的面板
    private void ShowCurrentPanel()
    {
        // 先隐藏所有面板
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }

        // 显示当前面板
        if (currentPanelIndex >= 0 && currentPanelIndex < panels.Count)
        {
            panels[currentPanelIndex].SetActive(true);
        }
    }
}
