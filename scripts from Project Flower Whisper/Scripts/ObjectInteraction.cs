using UnityEngine;
using DG.Tweening;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject prefabToInstantiate; // 存储的Prefab
    public GameObject targetObjectB; // 目标物体B
    public string targetTag; // 要检测的特定Tag
    public Vector3 startLocalPosition; // 起始位置的局部坐标（相对于父对象）
    public Vector3 endLocalPosition; // 最终位置的局部坐标（相对于父对象）
    public float moveDuration = 1f; // 移动的持续时间

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

        // 实例化Prefab
        GameObject newChild = Instantiate(prefabToInstantiate);

        // 设置父对象
        newChild.transform.SetParent(targetObjectB.transform, false);

        // 设置生成物体的局部坐标位置为startLocalPosition
        newChild.transform.localPosition = startLocalPosition;

        // 使用DoTween将Prefab从起始位置移动到最终位置
        newChild.transform.DOLocalMove(endLocalPosition, moveDuration).SetEase(Ease.OutQuad);
    }
}
