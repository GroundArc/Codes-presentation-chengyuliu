using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContainerEffect : MonoBehaviour
{
    public GameObject uiImage; // ������ʾ��UIͼƬ��Ĭ�Ϲرգ�
    public GameObject particlePrefab; // ����Ч����Ԥ����
    public float floatAmount = 0.5f; // ����Ʈ���ķ���
    public float floatDuration = 2f; // Ʈ����ʱ��

    public Transform targetObject; // �ֶ�ָ����Ŀ������

    public bool isEffectActive = false; // ׷��Ч���Ƿ��Ѿ�����
    public Transform containerChild; // �����ҵ����Ӷ���

    void Start()
    {
        // ��ʼ��ʱȷ��UIͼƬ�ǹرյ�
        if (uiImage != null)
        {
            uiImage.SetActive(false);
            Debug.Log("UI Image is initially set to inactive.");
        }
        else
        {
            Debug.LogWarning("UI Image is not assigned.");
        }
    }

    void Update()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return;
        }

        // ʵʱ���Ŀ�������Ƿ����κ��Ӷ���
        if (targetObject.childCount > 0 && !isEffectActive)
        {
            Debug.Log("Target object has " + targetObject.childCount + " child object(s).");

            // ȡ�õ�һ���Ӷ���
            containerChild = targetObject.GetChild(0);
            Debug.Log("Using the first child object: " + containerChild.name);

            // ��ʾUIͼƬ
            if (uiImage != null)
            {
                uiImage.SetActive(true);
                Debug.Log("UI Image is now active.");
            }

            // �������Ч��
            if (particlePrefab != null)
            {
                GameObject particleEffect = Instantiate(particlePrefab, containerChild.position, Quaternion.identity, containerChild);
                particleEffect.transform.localPosition = Vector3.zero; // ����Ч����λ�����Ӷ������
                Debug.Log("Particle effect instantiated at: " + containerChild.position);
            }
            else
            {
                Debug.LogWarning("Particle prefab is not assigned.");
            }

            // ��ʼ����Ʈ��Ч��
            StartFloating(containerChild);
            isEffectActive = true;
        }
        else if (targetObject.childCount == 0 && isEffectActive)
        {
            // ���û���Ӷ�����Ч���Ѿ������ر�UI��Ч��
            Debug.Log("Target object has no child objects, disabling effects.");

            if (uiImage != null)
            {
                uiImage.SetActive(false);
            }

            if (containerChild != null)
            {
                containerChild.DOKill(); // ֹͣƮ��Ч��
                containerChild = null;
            }

            isEffectActive = false;
        }
    }

    void StartFloating(Transform target)
    {
        Debug.Log("Starting floating effect on: " + target.name);
        // ����Ʈ��Ч��
        target.DOMoveY(target.position.y + floatAmount, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // ����ѭ����Y������Ʈ��
    }
}
