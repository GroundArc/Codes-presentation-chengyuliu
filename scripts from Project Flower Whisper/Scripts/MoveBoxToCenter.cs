using UnityEngine;
using DG.Tweening;

public class MoveBoxToCenter : MonoBehaviour
{
    public RectTransform boxTransform; // 主盒子的RectTransform
    public RectTransform childBoxTransform; // 子对象（小盒子）的RectTransform
    public float duration = 1f; // 动画持续时间
    public Vector3 childTargetPosition; // 子对象的目标位置
    

    public Vector3 originalPosition; // 主盒子的原始位置（左下角）
    public Vector3 targetPosition; // 目标位置（屏幕中央）
    public Vector3 rightBottomPosition; // 右下角位置
    public Vector3 childOriginalPosition; // 子对象的原始位置

    public GameObject Tnoti;

    void Start()
    {
        // 获取主盒子的原始位置（屏幕左下角）
        originalPosition = boxTransform.localPosition;

        // 设置目标位置为屏幕中央
        targetPosition = new Vector3(0, -120, originalPosition.z);

        // 设置右下角位置
        rightBottomPosition = new Vector3(Screen.width / 2f, -Screen.height / 2f, originalPosition.z);

        // 获取子对象的原始位置
        childOriginalPosition = childBoxTransform.localPosition;
    }

    void Update()
    {
        // 当按下T键时，启动动画
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (boxTransform.localPosition == targetPosition)
            {
                // 如果盒子已经在屏幕中央，先恢复子对象位置再移动到右下角
                ReturnChildAndMoveBox();
            }
            else
            {
                // 否则，将盒子移动到屏幕中央并旋转360度
                MoveAndRotateBoxToCenter();
            }
        }
    }

    void MoveAndRotateBoxToCenter()
    {
        // 使用DOTween将盒子从原始位置移动到目标位置
        boxTransform.DOLocalMove(targetPosition, duration).SetEase(Ease.OutSine);

        // 使用DOTween让盒子在Y轴上旋转360度
        boxTransform.DOLocalRotate(new Vector3(0, 540, 0), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() => MoveChildBox()); // 在动画结束后移动子对象
        Tnoti.SetActive(true); 

    }

    void MoveChildBox()
    {
        // 使用DOTween将子对象移动到特定位置
        childBoxTransform.DOLocalMove(childTargetPosition, duration).SetEase(Ease.OutSine);
    }

    void ReturnChildAndMoveBox()
    {
        // 使用DOTween将子对象返回到原始位置
        childBoxTransform.DOLocalMove(childOriginalPosition, duration).SetEase(Ease.OutSine)
                        .OnComplete(() => MoveAndRotateBoxToRightBottom()); // 在子对象恢复位置后移动主盒子
    }

    void MoveAndRotateBoxToRightBottom()
    {
        // 使用DOTween将盒子移动到右下角并旋转540度
        boxTransform.DOLocalMove(rightBottomPosition, duration).SetEase(Ease.InSine);
        boxTransform.DOLocalRotate(new Vector3(0, 540, 0), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.InSine)
                    .OnComplete(() => ResetBoxPosition()); // 在动画结束后重置位置
        Tnoti.SetActive(false);

    }

    void ResetBoxPosition()
    {
        // 将盒子的位置立即重置为原始位置
        boxTransform.localPosition = originalPosition;
    }
}
