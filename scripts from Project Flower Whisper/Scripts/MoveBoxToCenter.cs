using UnityEngine;
using DG.Tweening;

public class MoveBoxToCenter : MonoBehaviour
{
    public RectTransform boxTransform; // �����ӵ�RectTransform
    public RectTransform childBoxTransform; // �Ӷ���С���ӣ���RectTransform
    public float duration = 1f; // ��������ʱ��
    public Vector3 childTargetPosition; // �Ӷ����Ŀ��λ��
    

    public Vector3 originalPosition; // �����ӵ�ԭʼλ�ã����½ǣ�
    public Vector3 targetPosition; // Ŀ��λ�ã���Ļ���룩
    public Vector3 rightBottomPosition; // ���½�λ��
    public Vector3 childOriginalPosition; // �Ӷ����ԭʼλ��

    public GameObject Tnoti;

    void Start()
    {
        // ��ȡ�����ӵ�ԭʼλ�ã���Ļ���½ǣ�
        originalPosition = boxTransform.localPosition;

        // ����Ŀ��λ��Ϊ��Ļ����
        targetPosition = new Vector3(0, -120, originalPosition.z);

        // �������½�λ��
        rightBottomPosition = new Vector3(Screen.width / 2f, -Screen.height / 2f, originalPosition.z);

        // ��ȡ�Ӷ����ԭʼλ��
        childOriginalPosition = childBoxTransform.localPosition;
    }

    void Update()
    {
        // ������T��ʱ����������
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (boxTransform.localPosition == targetPosition)
            {
                // ��������Ѿ�����Ļ���룬�Ȼָ��Ӷ���λ�����ƶ������½�
                ReturnChildAndMoveBox();
            }
            else
            {
                // ���򣬽������ƶ�����Ļ���벢��ת360��
                MoveAndRotateBoxToCenter();
            }
        }
    }

    void MoveAndRotateBoxToCenter()
    {
        // ʹ��DOTween�����Ӵ�ԭʼλ���ƶ���Ŀ��λ��
        boxTransform.DOLocalMove(targetPosition, duration).SetEase(Ease.OutSine);

        // ʹ��DOTween�ú�����Y������ת360��
        boxTransform.DOLocalRotate(new Vector3(0, 540, 0), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() => MoveChildBox()); // �ڶ����������ƶ��Ӷ���
        Tnoti.SetActive(true); 

    }

    void MoveChildBox()
    {
        // ʹ��DOTween���Ӷ����ƶ����ض�λ��
        childBoxTransform.DOLocalMove(childTargetPosition, duration).SetEase(Ease.OutSine);
    }

    void ReturnChildAndMoveBox()
    {
        // ʹ��DOTween���Ӷ��󷵻ص�ԭʼλ��
        childBoxTransform.DOLocalMove(childOriginalPosition, duration).SetEase(Ease.OutSine)
                        .OnComplete(() => MoveAndRotateBoxToRightBottom()); // ���Ӷ���ָ�λ�ú��ƶ�������
    }

    void MoveAndRotateBoxToRightBottom()
    {
        // ʹ��DOTween�������ƶ������½ǲ���ת540��
        boxTransform.DOLocalMove(rightBottomPosition, duration).SetEase(Ease.InSine);
        boxTransform.DOLocalRotate(new Vector3(0, 540, 0), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.InSine)
                    .OnComplete(() => ResetBoxPosition()); // �ڶ�������������λ��
        Tnoti.SetActive(false);

    }

    void ResetBoxPosition()
    {
        // �����ӵ�λ����������Ϊԭʼλ��
        boxTransform.localPosition = originalPosition;
    }
}
