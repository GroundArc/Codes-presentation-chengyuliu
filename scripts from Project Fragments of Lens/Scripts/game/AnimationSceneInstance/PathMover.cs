using UnityEngine;
using DG.Tweening; // ���� DOTween �����ռ�
using System.Collections.Generic;

public class PathMover : MonoBehaviour
{
    [System.Serializable]
    public class PathElement
    {
        public Vector3 position; // λ��
        public Vector3 rotation; // ��ת��ŷ���ǣ�
    }

    public List<PathElement> pathElements = new List<PathElement>(); // ·���ڵ��б�
    public float moveSpeed = 1f; // �����ƶ��ٶ�
    public Ease moveEase = Ease.Linear; // �����ƶ��Ļ�������

    public GameObject specialUIElement; // �ض��� UI Ԫ��
    [SerializeField] private UIElementMover uiElementMover; // UIElementMover ������

    private int currentElementIndex = 0; // ��ǰ��·���ڵ�����
    private bool isMoving = false;
    private bool isReturning = false; // ����·������ı���

    void Start()
    {
        if (uiElementMover == null)
        {
            Debug.LogError("UIElementMover is not assigned in the Inspector.");
        }
    }

    // ��ʼ�ƶ����������� isReturning ȷ��˳������ƶ�
    public void StartMoving()
    {
        if (isMoving || pathElements.Count == 0) return;

        isMoving = true;

        // ����·����ʼʱ�� UI ����
        OnStartPath();

        if (isReturning)
        {
            currentElementIndex = pathElements.Count - 1;
            MoveToPreviousElement();
        }
        else
        {
            currentElementIndex = 0;
            MoveToNextElement();
        }
    }

    // ˳���ƶ�����һ���ڵ�
    private void MoveToNextElement()
    {
        if (currentElementIndex >= pathElements.Count)
        {
            isMoving = false;
            isReturning = true; // ���Ϊ����״̬
            OnEndPath(); // ·�����ʱ�� UI ����
            return;
        }

        PathElement targetElement = pathElements[currentElementIndex];
        float distance = Vector3.Distance(transform.position, targetElement.position);
        float duration = distance / moveSpeed;

        transform.DOMove(targetElement.position, duration).SetEase(moveEase);
        transform.DORotate(targetElement.rotation, duration).SetEase(moveEase).OnComplete(() =>
        {
            currentElementIndex++;
            MoveToNextElement();
        });
    }

    // �����ƶ�����һ���ڵ�
    private void MoveToPreviousElement()
    {
        if (currentElementIndex < 0)
        {
            isMoving = false;
            isReturning = false; // ����Ϊ˳��״̬

            return;
        }

        PathElement targetElement = pathElements[currentElementIndex];
        float distance = Vector3.Distance(transform.position, targetElement.position);
        float duration = distance / moveSpeed;

        transform.DOMove(targetElement.position, duration).SetEase(moveEase);
        transform.DORotate(targetElement.rotation, duration).SetEase(moveEase).OnComplete(() =>
        {
            currentElementIndex--;
            MoveToPreviousElement();
        });
    }

    // ·����ʼʱ UI ����
    private void OnStartPath()
    {
        if (uiElementMover == null) return;

        uiElementMover.RemoveUIElement(specialUIElement);
        specialUIElement.SetActive(false);
    }

    // ·������ʱ UI ����
    private void OnEndPath()
    {
        if (uiElementMover == null) return;

        specialUIElement.SetActive(true);
        uiElementMover.AddUIElement(specialUIElement);
    }
}
