using UnityEngine;
using DG.Tweening; // 引入Dotween命名空间
using System.Collections;

public class MoveAndRotate : MonoBehaviour
{
    // 在Inspector中可输入的新位置和旋转
    [Header("New Transform Settings")]
    public Vector3 newPosition;
    public Vector3 newRotation;

    [Header("Tween Settings")]
    public float duration = 2f; // 动画持续时间
    public Ease easeType = Ease.Linear; // 动画曲线类型

    [Header("Child Object Settings")]
    public float childMoveDistance = 2f; // 每个子对象移动的距离
    public float childMoveDuration = 0.5f; // 每个子对象的移动时间
    public float delayBetweenChildren = 0.2f; // 每个子对象之间的延迟

    [Header("Page Reader Reference")]
    public PageReader pageReader; // 引用 PageReader

    private Vector3 originalPosition; // 父对象的初始位置
    private Vector3 originalRotation; // 父对象的初始旋转

    private void Start()
    {
        originalPosition = transform.localPosition; // 存储父对象的初始位置
        originalRotation = transform.localEulerAngles; // 存储父对象的初始旋转
    }

    // 方法：使用Dotween移动和旋转物体
    public void MoveToNewPositionAndRotation()
    {
        // 平滑移动到新的位置
        transform.DOLocalMove(newPosition, duration).SetEase(easeType);

        // 平滑旋转到新的角度
        transform.DOLocalRotate(newRotation, duration).SetEase(easeType)
            .OnComplete(() =>
            {
                // 在旋转完成后开始移动子对象并启动协程
                MoveChildrenSequentially();
            });
    }

    // 方法：依次向右移动八个子对象
    private void MoveChildrenSequentially()
    {
        int childCount = Mathf.Min(transform.childCount, 8); // 只处理前八个子对象

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // 将子对象向右平移指定距离，最后一个子对象完成后调用 PageReader 初始化
            child.DOLocalMoveX(child.localPosition.x + childMoveDistance, childMoveDuration)
                .SetEase(easeType)
                .SetDelay(i * delayBetweenChildren)
                //.OnComplete(pageReader.ShowNextPage)
                ;
        }
    }

    // 新增方法：将子对象和父对象反向移动回原来的位置
    public void ReverseMoveAndRotate()
    {
        int childCount = Mathf.Min(transform.childCount, 8); // 处理前八个子对象
        Sequence sequence = DOTween.Sequence(); // 使用DoTween的Sequence来确保顺序执行

        // 依次将子对象移动回原始位置
        for (int i = childCount - 1; i >= 0; i--) // 倒序遍历
        {
            Transform child = transform.GetChild(i);
            Vector3 originalChildPosition = new Vector3(child.localPosition.x - childMoveDistance, child.localPosition.y, child.localPosition.z);

            sequence.Insert(i * delayBetweenChildren, child.DOLocalMoveX(originalChildPosition.x, childMoveDuration).SetEase(easeType));
        }

        // 当所有子对象完成移动后，再将父对象移动回原位置
        sequence.OnComplete(() =>
        {
            // 父对象平滑移动回原始位置并旋转回原始角度
            transform.DOLocalMove(originalPosition, duration).SetEase(easeType);
            transform.DOLocalRotate(originalRotation, duration).SetEase(easeType);
        });
    }
}
