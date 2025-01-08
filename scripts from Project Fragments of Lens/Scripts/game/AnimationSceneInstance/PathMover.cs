using UnityEngine;
using DG.Tweening; // 引入 DOTween 命名空间
using System.Collections.Generic;

public class PathMover : MonoBehaviour
{
    [System.Serializable]
    public class PathElement
    {
        public Vector3 position; // 位置
        public Vector3 rotation; // 旋转（欧拉角）
    }

    public List<PathElement> pathElements = new List<PathElement>(); // 路径节点列表
    public float moveSpeed = 1f; // 控制移动速度
    public Ease moveEase = Ease.Linear; // 控制移动的缓动类型

    public GameObject specialUIElement; // 特定的 UI 元素
    [SerializeField] private UIElementMover uiElementMover; // UIElementMover 的引用

    private int currentElementIndex = 0; // 当前的路径节点索引
    private bool isMoving = false;
    private bool isReturning = false; // 控制路径方向的变量

    void Start()
    {
        if (uiElementMover == null)
        {
            Debug.LogError("UIElementMover is not assigned in the Inspector.");
        }
    }

    // 开始移动方法，根据 isReturning 确定顺序或倒序移动
    public void StartMoving()
    {
        if (isMoving || pathElements.Count == 0) return;

        isMoving = true;

        // 进行路径开始时的 UI 处理
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

    // 顺序移动到下一个节点
    private void MoveToNextElement()
    {
        if (currentElementIndex >= pathElements.Count)
        {
            isMoving = false;
            isReturning = true; // 标记为返回状态
            OnEndPath(); // 路径完成时的 UI 处理
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

    // 倒序移动到上一个节点
    private void MoveToPreviousElement()
    {
        if (currentElementIndex < 0)
        {
            isMoving = false;
            isReturning = false; // 重置为顺序状态

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

    // 路径开始时 UI 处理
    private void OnStartPath()
    {
        if (uiElementMover == null) return;

        uiElementMover.RemoveUIElement(specialUIElement);
        specialUIElement.SetActive(false);
    }

    // 路径结束时 UI 处理
    private void OnEndPath()
    {
        if (uiElementMover == null) return;

        specialUIElement.SetActive(true);
        uiElementMover.AddUIElement(specialUIElement);
    }
}
