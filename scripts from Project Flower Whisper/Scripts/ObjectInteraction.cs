using UnityEngine;
using DG.Tweening;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject prefabToInstantiate; // �洢��Prefab
    public GameObject targetObjectB; // Ŀ������B
    public string targetTag; // Ҫ�����ض�Tag
    public Vector3 startLocalPosition; // ��ʼλ�õľֲ����꣨����ڸ�����
    public Vector3 endLocalPosition; // ����λ�õľֲ����꣨����ڸ�����
    public float moveDuration = 1f; // �ƶ��ĳ���ʱ��

    void Update()
    {
        // ���������¼�
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����Ƿ���������A
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
        // ���Ŀ������B�Ƿ���ھ����ض�Tag���Ӷ���
        Transform existingChild = null;
        foreach (Transform child in targetObjectB.transform)
        {
            if (child.CompareTag(targetTag))
            {
                existingChild = child;
                break;
            }
        }

        // ������ڣ�ɾ�����Ӷ���
        if (existingChild != null)
        {
            Destroy(existingChild.gameObject);
        }

        // ʵ����Prefab
        GameObject newChild = Instantiate(prefabToInstantiate);

        // ���ø�����
        newChild.transform.SetParent(targetObjectB.transform, false);

        // ������������ľֲ�����λ��ΪstartLocalPosition
        newChild.transform.localPosition = startLocalPosition;

        // ʹ��DoTween��Prefab����ʼλ���ƶ�������λ��
        newChild.transform.DOLocalMove(endLocalPosition, moveDuration).SetEase(Ease.OutQuad);
    }
}
