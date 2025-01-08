using UnityEngine;
using DG.Tweening; // ����Dotween�����ռ�
using System.Collections;

public class MoveAndRotate : MonoBehaviour
{
    // ��Inspector�п��������λ�ú���ת
    [Header("New Transform Settings")]
    public Vector3 newPosition;
    public Vector3 newRotation;

    [Header("Tween Settings")]
    public float duration = 2f; // ��������ʱ��
    public Ease easeType = Ease.Linear; // ������������

    [Header("Child Object Settings")]
    public float childMoveDistance = 2f; // ÿ���Ӷ����ƶ��ľ���
    public float childMoveDuration = 0.5f; // ÿ���Ӷ�����ƶ�ʱ��
    public float delayBetweenChildren = 0.2f; // ÿ���Ӷ���֮����ӳ�

    [Header("Page Reader Reference")]
    public PageReader pageReader; // ���� PageReader

    private Vector3 originalPosition; // ������ĳ�ʼλ��
    private Vector3 originalRotation; // ������ĳ�ʼ��ת

    private void Start()
    {
        originalPosition = transform.localPosition; // �洢������ĳ�ʼλ��
        originalRotation = transform.localEulerAngles; // �洢������ĳ�ʼ��ת
    }

    // ������ʹ��Dotween�ƶ�����ת����
    public void MoveToNewPositionAndRotation()
    {
        // ƽ���ƶ����µ�λ��
        transform.DOLocalMove(newPosition, duration).SetEase(easeType);

        // ƽ����ת���µĽǶ�
        transform.DOLocalRotate(newRotation, duration).SetEase(easeType)
            .OnComplete(() =>
            {
                // ����ת��ɺ�ʼ�ƶ��Ӷ�������Э��
                MoveChildrenSequentially();
            });
    }

    // ���������������ƶ��˸��Ӷ���
    private void MoveChildrenSequentially()
    {
        int childCount = Mathf.Min(transform.childCount, 8); // ֻ����ǰ�˸��Ӷ���

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // ���Ӷ�������ƽ��ָ�����룬���һ���Ӷ�����ɺ���� PageReader ��ʼ��
            child.DOLocalMoveX(child.localPosition.x + childMoveDistance, childMoveDuration)
                .SetEase(easeType)
                .SetDelay(i * delayBetweenChildren)
                //.OnComplete(pageReader.ShowNextPage)
                ;
        }
    }

    // �������������Ӷ���͸��������ƶ���ԭ����λ��
    public void ReverseMoveAndRotate()
    {
        int childCount = Mathf.Min(transform.childCount, 8); // ����ǰ�˸��Ӷ���
        Sequence sequence = DOTween.Sequence(); // ʹ��DoTween��Sequence��ȷ��˳��ִ��

        // ���ν��Ӷ����ƶ���ԭʼλ��
        for (int i = childCount - 1; i >= 0; i--) // �������
        {
            Transform child = transform.GetChild(i);
            Vector3 originalChildPosition = new Vector3(child.localPosition.x - childMoveDistance, child.localPosition.y, child.localPosition.z);

            sequence.Insert(i * delayBetweenChildren, child.DOLocalMoveX(originalChildPosition.x, childMoveDuration).SetEase(easeType));
        }

        // �������Ӷ�������ƶ����ٽ��������ƶ���ԭλ��
        sequence.OnComplete(() =>
        {
            // ������ƽ���ƶ���ԭʼλ�ò���ת��ԭʼ�Ƕ�
            transform.DOLocalMove(originalPosition, duration).SetEase(easeType);
            transform.DOLocalRotate(originalRotation, duration).SetEase(easeType);
        });
    }
}
