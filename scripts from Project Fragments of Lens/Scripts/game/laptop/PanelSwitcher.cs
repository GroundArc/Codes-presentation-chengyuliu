using UnityEngine;
using System.Collections.Generic;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject emptyMessagePanel;      // ��ʾ�򣬵��б�Ϊ��ʱ��ʾ
    public List<GameObject> panels = new List<GameObject>(); // �ɶ�̬��Ӻ�ɾ��������б�

    private int currentPanelIndex = -1; // ��ǰ��ʾ�����������-1��ʾ�������ʾ

    // ��ʼ��ʱ����б��Ƿ�Ϊ��
    void Start()
    {
        UpdateEmptyMessagePanel();
        ShowCurrentPanel();
    }

    // ��ʾ��һ�����
    public void ShowNextPanel()
    {
        if (panels.Count == 0) return; // �б�Ϊ���򲻲���

        currentPanelIndex = (currentPanelIndex + 1) % panels.Count; // ѭ������һ������
        ShowCurrentPanel();
    }

    // ��ʾ��һ�����
    public void ShowPreviousPanel()
    {
        if (panels.Count == 0) return; // �б�Ϊ���򲻲���

        currentPanelIndex = (currentPanelIndex - 1 + panels.Count) % panels.Count; // ѭ������һ������
        ShowCurrentPanel();
    }

    // ����µ����
    public void AddPanel(GameObject newPanel)
    {
        panels.Add(newPanel); // ������
        UpdateEmptyMessagePanel(); // ����Ƿ���ʾ��ʾ��
        if (currentPanelIndex == -1) currentPanelIndex = 0; // ����Ϊ��һ�����
        ShowCurrentPanel();
    }

    // ɾ�����
    public void RemovePanel(GameObject panelToRemove)
    {
        if (panels.Contains(panelToRemove))
        {
            panels.Remove(panelToRemove); // �Ƴ����
            UpdateEmptyMessagePanel(); // ����Ƿ���ʾ��ʾ��

            // ������ǰ��ʾ���������
            if (currentPanelIndex >= panels.Count)
            {
                currentPanelIndex = panels.Count - 1;
            }
            ShowCurrentPanel();
        }
    }

    // ������ʾ�����ʾ״̬
    private void UpdateEmptyMessagePanel()
    {
        if (emptyMessagePanel != null)
        {
            emptyMessagePanel.SetActive(panels.Count == 0); // ���б�Ϊ��ʱ��ʾ��ʾ��
        }
    }

    // ��ʾ��ǰ���������
    private void ShowCurrentPanel()
    {
        // �������������
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }

        // ��ʾ��ǰ���
        if (currentPanelIndex >= 0 && currentPanelIndex < panels.Count)
        {
            panels[currentPanelIndex].SetActive(true);
        }
    }
}
