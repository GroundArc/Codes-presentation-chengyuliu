using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPos : MonoBehaviour
{
    public Collider containerCollider; // Container ����ײ��
    public Transform tray; // FlowerTray ����
    public Transform container; // Container ����

    private FlowerManager flowerManager;
    private FlowerBehaviour flowerBehaviour;
    private Vector3 initialPosition;
    private Transform initialParent;
    private Vector3 originalScale; // ԭʼ���ű���
    private bool isInTray = false; // ��־λ����ʾ�Ƿ�������������
    private bool hasLeftTray = false; // ��־λ����ʾ�Ƿ��Ѿ��뿪��������

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

        // ��ʼ��λ�á�����������ű���
        initialPosition = transform.position;
        initialParent = transform.parent;
        originalScale = transform.localScale;

        // ��ʼ��黨��λ��
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
            // ������������
            if (!isInTray || transform.parent != tray)
            {
                isInTray = true;
                SetParentWithReset(transform, tray, originalScale);

                // ֻ�е����Ѿ��뿪����������ʱ��������������ӵ�FlowerManager
                if (hasLeftTray && !flowerManager.flowers.Contains(flowerBehaviour.flowerData))
                {
                    flowerManager.Add(flowerBehaviour.flowerData);
                    Debug.Log("Flower in tray, added to FlowerManager: " + flowerBehaviour.flowerData.flowerName);
                }
            }
        }
        else
        {
            // �����������ڻ����κ�������
            if (isInTray || transform.parent != container)
            {
                isInTray = false;
                SetParentWithReset(transform, container, Vector3.one);

                if (flowerManager.flowers.Contains(flowerBehaviour.flowerData))
                {
                    flowerManager.Remove(flowerBehaviour.flowerData);
                    hasLeftTray = true; // ��ǻ����Ѿ��뿪����������
                    Debug.Log("Flower in container, removed from FlowerManager: " + flowerBehaviour.flowerData.flowerName);
                }
            }
        }
    }

    private void SetParentWithReset(Transform child, Transform parent, Vector3 scale)
    {
        // ��ȡ��ǰȫ��λ��
        Vector3 globalPosition = child.position;
        Quaternion globalRotation = child.rotation;

        // �����µĸ�����
        child.SetParent(parent);

        // ��������ȫ��λ�ú���ת
        child.position = globalPosition;
        child.localScale = scale;
        child.rotation = globalRotation;

        Debug.Log("Set parent to " + parent.name);
    }
}
