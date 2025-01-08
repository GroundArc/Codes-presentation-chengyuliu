using UnityEngine;
using DG.Tweening;

public class MoveAndDisable : MonoBehaviour
{
    public ParticleSystem particleSystemToFade; // 需要逐渐减小Emission的ParticleSystem
    public float moveDuration = 1f; // 物体移动的持续时间
    public float moveDistance = 10f; // 物体在Y轴上移动的距离
    public float emissionFadeDuration = 1f; // Emission减小到0的持续时间

    // 调用这个方法来使物体丝滑向上移动并禁用
    public void MoveUpAndDisable()
    {
        if (particleSystemToFade != null)
        {
            // 获取ParticleSystem的Emission模块
            var emission = particleSystemToFade.emission;

            // 获取当前Emission的Rate over Time
            float initialRate = emission.rateOverTime.constant;

            // 通过DOTween将Emission的Rate over Time逐渐减少到0
            DOTween.To(() => initialRate, x => emission.rateOverTime = x, 0, emissionFadeDuration)
                .OnComplete(() => StartMove()); // 在Emission减小到0后，开始移动物体
        }
        else
        {
            // 如果没有ParticleSystem，直接移动物体
            StartMove();
        }
    }

    private void StartMove()
    {
        // 使用DoTween将物体沿Y轴向上移动
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine) // 使用平滑的缓入缓出效果
            .OnComplete(() => gameObject.SetActive(false)); // 移动完成后禁用物体
    }
}
