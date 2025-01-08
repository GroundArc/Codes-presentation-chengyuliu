using UnityEngine;
using DG.Tweening;
using TMPro;

public class ObjectInteractionT : MonoBehaviour
{
    public GameObject prefabToInstantiate; // 存储的Prefab
    public GameObject targetObjectB; // 目标物体B
    public Transform endTransform; // 最终位置的Transform
    public string targetTag; // 要检测的特定Tag
    public TMP_Text warningText; // 提示文本的TMP组件
    public float moveDuration = 1f; // 移动的持续时间
    public float warningDuration = 2f; // 提示文本显示的持续时间

    void Start()
    {
        // 初始化时隐藏提示文本
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 检测鼠标点击事件
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 检查是否点击了物体A
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    HandleObjectB();
                }
            }
        }
    }

    void HandleObjectB()
    {
        // 检查targetObjectB是否包含Tag为“Wrapper”的子对象
        bool hasWrapper = false;
        foreach (Transform child in targetObjectB.transform)
        {
            if (child.CompareTag("Wrapper"))
            {
                hasWrapper = true;
                break;
            }
        }

        if (!hasWrapper)
        {
            // 如果没有找到“Wrapper”子对象，显示警告并震动
            ShowWarning("Need to Select Wrapper first");
            return;
        }

        // 检查目标物体B是否存在具有特定Tag的子对象
        Transform existingChild = null;
        foreach (Transform child in targetObjectB.transform)
        {
            if (child.CompareTag(targetTag))
            {
                existingChild = child;
                break;
            }
        }

        // 如果存在，删除该子对象
        if (existingChild != null)
        {
            Destroy(existingChild.gameObject);
        }

        // 计算物体A的位置在物体B局部坐标系中的位置
        Vector3 startLocalPosition = targetObjectB.transform.InverseTransformPoint(this.transform.position);

        // 实例化Prefab
        GameObject newChild = Instantiate(prefabToInstantiate);

        // 设置父对象
        newChild.transform.SetParent(targetObjectB.transform, false);

        // 设置生成物体的局部坐标位置为物体A的对应位置
        newChild.transform.localPosition = startLocalPosition;

        // 使用DoTween将Prefab从起始位置移动到endTransform指定的位置
        newChild.transform.DOLocalMove(targetObjectB.transform.InverseTransformPoint(endTransform.position), moveDuration).SetEase(Ease.OutQuad);
        newChild.transform.DORotate(new Vector3(360, 0, 0), moveDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
    }

    void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningText.gameObject.SetActive(true);

            // 让提示文本通过DoTween震动
            warningText.rectTransform.DOShakePosition(0.5f, new Vector3(5f, 5f, 0), 10, 90, false, true);

            // 设置2秒后自动关闭提示
            DOVirtual.DelayedCall(warningDuration, () => warningText.gameObject.SetActive(false));
        }
    }
}
