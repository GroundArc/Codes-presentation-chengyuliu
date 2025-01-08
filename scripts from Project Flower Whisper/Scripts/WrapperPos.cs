using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperPos : MonoBehaviour
{
    public Collider containerCollider; // Container ����ײ��
    public Transform tray; // WrapperTray ����
    public Transform container; // Container ����

    private WrapperManager wrapperManager;
    private WrapperBehaviour wrapperBehaviour;
    private Vector3 initialPosition;
    private Transform initialParent;
    private Vector3 originalScale; // ԭʼ���ű���
    private bool isInTray = false; // ��־λ����ʾ�Ƿ�������������

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

        // ��ʼ��λ�á�����������ű���
        initialPosition = transform.position;
        initialParent = transform.parent;
        originalScale = transform.localScale;

        // ��ʼ����װλ��
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
            // ������������
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
            // �����������ڻ����κ�������
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
