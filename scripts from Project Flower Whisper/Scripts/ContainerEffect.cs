using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContainerEffect : MonoBehaviour
{
    public GameObject uiImage; // 用于显示的UI图片（默认关闭）
    public GameObject particlePrefab; // 粒子效果的预制体
    public float floatAmount = 0.5f; // 上下飘荡的幅度
    public float floatDuration = 2f; // 飘荡的时间

    public Transform targetObject; // 手动指定的目标物体

    public bool isEffectActive = false; // 追踪效果是否已经激活
    public Transform containerChild; // 缓存找到的子对象

    void Start()
    {
        // 初始化时确保UI图片是关闭的
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

        // 实时检查目标物体是否有任何子对象
        if (targetObject.childCount > 0 && !isEffectActive)
        {
            Debug.Log("Target object has " + targetObject.childCount + " child object(s).");

            // 取得第一个子对象
            containerChild = targetObject.GetChild(0);
            Debug.Log("Using the first child object: " + containerChild.name);

            // 显示UI图片
            if (uiImage != null)
            {
                uiImage.SetActive(true);
                Debug.Log("UI Image is now active.");
            }

            // 添加粒子效果
            if (particlePrefab != null)
            {
                GameObject particleEffect = Instantiate(particlePrefab, containerChild.position, Quaternion.identity, containerChild);
                particleEffect.transform.localPosition = Vector3.zero; // 粒子效果的位置与子对象对齐
                Debug.Log("Particle effect instantiated at: " + containerChild.position);
            }
            else
            {
                Debug.LogWarning("Particle prefab is not assigned.");
            }

            // 开始上下飘荡效果
            StartFloating(containerChild);
            isEffectActive = true;
        }
        else if (targetObject.childCount == 0 && isEffectActive)
        {
            // 如果没有子对象并且效果已经激活，则关闭UI和效果
            Debug.Log("Target object has no child objects, disabling effects.");

            if (uiImage != null)
            {
                uiImage.SetActive(false);
            }

            if (containerChild != null)
            {
                containerChild.DOKill(); // 停止飘荡效果
                containerChild = null;
            }

            isEffectActive = false;
        }
    }

    void StartFloating(Transform target)
    {
        Debug.Log("Starting floating effect on: " + target.name);
        // 上下飘荡效果
        target.DOMoveY(target.position.y + floatAmount, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // 无限循环的Y轴上下飘荡
    }
}
