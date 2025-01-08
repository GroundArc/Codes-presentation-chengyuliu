using UnityEngine;
using DG.Tweening;

public class MoveAndDisable : MonoBehaviour
{
    public ParticleSystem particleSystemToFade; // ��Ҫ�𽥼�СEmission��ParticleSystem
    public float moveDuration = 1f; // �����ƶ��ĳ���ʱ��
    public float moveDistance = 10f; // ������Y�����ƶ��ľ���
    public float emissionFadeDuration = 1f; // Emission��С��0�ĳ���ʱ��

    // �������������ʹ����˿�������ƶ�������
    public void MoveUpAndDisable()
    {
        if (particleSystemToFade != null)
        {
            // ��ȡParticleSystem��Emissionģ��
            var emission = particleSystemToFade.emission;

            // ��ȡ��ǰEmission��Rate over Time
            float initialRate = emission.rateOverTime.constant;

            // ͨ��DOTween��Emission��Rate over Time�𽥼��ٵ�0
            DOTween.To(() => initialRate, x => emission.rateOverTime = x, 0, emissionFadeDuration)
                .OnComplete(() => StartMove()); // ��Emission��С��0�󣬿�ʼ�ƶ�����
        }
        else
        {
            // ���û��ParticleSystem��ֱ���ƶ�����
            StartMove();
        }
    }

    private void StartMove()
    {
        // ʹ��DoTween��������Y�������ƶ�
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine) // ʹ��ƽ���Ļ��뻺��Ч��
            .OnComplete(() => gameObject.SetActive(false)); // �ƶ���ɺ��������
    }
}
