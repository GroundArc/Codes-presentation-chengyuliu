using UnityEngine;
using System.Collections.Generic;

public class UIElementMover : MonoBehaviour
{
    public List<GameObject> uiElements = new List<GameObject>(); // �洢���е� UI Ԫ��
    public List<Vector3> uiPositions = new List<Vector3>();      // �洢���е� UI λ��

    [Header("UI ��ʾ��")]
    public GameObject addUIPrompt; // ��ʾ UI ��ӵĿ�
    public GameObject removeUIPrompt; // ��ʾ UI ɾ���Ŀ�
    public GameObject emptyListPrompt; // ���б�Ϊ��ʱ��ʾ����ʾ����

    private void Start()
    {
        // ��ʼ��ʱ������ʾ��
        addUIPrompt.SetActive(false);
        removeUIPrompt.SetActive(false);
        UpdateEmptyListPrompt();
    }

    // ���һ���µ� UI Ԫ�أ��������ƶ�����Ӧ�Ķ���λ��
    public void AddUIElement(GameObject newUIElement)
    {
        if (uiElements.Count >= uiPositions.Count)
        {
            Debug.LogWarning("UIԪ������������λ������������λ���б�");
            return;
        }

        uiElements.Add(newUIElement);
        UpdateUIPositions();
        ShowAddUIPrompt();
        UpdateEmptyListPrompt(); // �����б�״̬
    }

    // ɾ��һ�� UI Ԫ�أ����ú�����Ԫ����ǰ�ƶ�
    public void RemoveUIElement(GameObject uiElementToRemove)
    {
        if (uiElements.Contains(uiElementToRemove))
        {
            uiElements.Remove(uiElementToRemove);
            UpdateUIPositions();
            ShowRemoveUIPrompt();
            UpdateEmptyListPrompt(); // �����б�״̬
        }
        else
        {
            Debug.LogWarning("����ɾ���� UI Ԫ�ز��������б��С�");
        }
    }

    // �������� UI Ԫ�ص�λ��
    private void UpdateUIPositions()
    {
        for (int i = 0; i < uiElements.Count; i++)
        {
            // ȷ��λ�ú�UIԪ������ƥ��
            if (i < uiPositions.Count)
            {
                uiElements[i].transform.localPosition = uiPositions[i];
            }
            else
            {
                Debug.LogWarning("λ���������㣬ĳЩ UI Ԫ���޷����뵽��ȷλ�á�");
            }
        }
    }

    // ��ʾ��� UI ����ʾ����ʾ 1 �������
    private void ShowAddUIPrompt()
    {
        addUIPrompt.SetActive(true);
    }


    // ��ʾɾ�� UI ����ʾ����ʾ 1 �������
    private void ShowRemoveUIPrompt()
    {
        removeUIPrompt.SetActive(true);
    }

    // �����б��Ƿ�Ϊ�գ����¿��б���ʾ������ʾ״̬
    private void UpdateEmptyListPrompt()
    {
        emptyListPrompt.SetActive(uiElements.Count == 0);
    }
}
