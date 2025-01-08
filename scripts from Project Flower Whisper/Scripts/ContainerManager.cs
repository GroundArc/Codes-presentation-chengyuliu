using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ContainerManager : MonoBehaviour
{
    public Transform containerPrefab; // Container��Ԥ����
    public Transform targetArea; // Ŀ�������Transform
    public Canvas floatingCanvas; // Ư�����ֵ�Canvas
    public TMP_Text warningText; // ��ʾ�ı���TextPro����
    public Camera mainCamera; // �������
    public Button copyButton; // ���ڴ������Ʋ����İ�ť
    public PlayerPickup playerPickup; // ����PlayerPickup�ű�

    private GameObject currentContainer; // ��ǰ���ڵ�Container

    private void Start()
    {
        if (copyButton != null)
        {
            copyButton.onClick.AddListener(CheckAndCopyContainer);
        }

        floatingCanvas.gameObject.SetActive(false); // ��ʼʱ����Ư������
        warningText.gameObject.SetActive(false); // ��ʼʱ���ؾ�������
    }

    private void Update()
    {
        // ��Ư������ʼ���泯���������
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

        // ���Container���Ӷ���Tag
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
            // ��ָ��TextPro����������ʾ
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
        // ��ʾ��������
        warningText.gameObject.SetActive(true);

        // ʹ��DoTween������Ч��
        warningText.transform.DOShakePosition(1f, 10, 10, 90, false, true);

        // 2������ؾ�������
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

        // ���containerPrefab���Ƿ����κ��Ӷ���
        if (containerPrefab.childCount == 0)
        {
            Debug.LogWarning("No child objects in the container to copy.");
            return;
        }

        // ȷ��Ŀ������ֻ����һ��Container
        if (currentContainer != null)
        {
            Destroy(currentContainer);
        }

        // �����µ�Container����
        currentContainer = Instantiate(containerPrefab.gameObject, targetArea.position, Quaternion.Euler(-90, 0, 0));
        currentContainer.name = containerPrefab.name + "_Copy";

        // �Ƴ� FlowerLanguageDisplay ���
        FlowerLanguageDisplay languageDisplay = currentContainer.GetComponent<FlowerLanguageDisplay>();
        if (languageDisplay != null)
        {
            Destroy(languageDisplay);
        }

        // ����PlayerPickup�е�currentContainer
        playerPickup.UpdateCurrentContainer(currentContainer);

        // ��ʾƯ������
        floatingCanvas.gameObject.SetActive(true);
       

        // ���ԭContainer���Ӷ���
        ClearContainerChildren(containerPrefab);
    }

    private void ClearContainerChildren(Transform containerTransform)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in containerTransform)
        {
            // ����δ���Ϊ "UnDelete" ���Ӷ�����ӵ���ɾ���б���
            if (!child.CompareTag("UnDelete"))
            {
                children.Add(child);
            }
        }

        // ɾ���������ռ������Ӷ���
        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }
    }

}
