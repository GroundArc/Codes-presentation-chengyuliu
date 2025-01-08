using UnityEngine;
using DG.Tweening;
using TMPro;

public class ObjectInteractionT : MonoBehaviour
{
    public GameObject prefabToInstantiate; // �洢��Prefab
    public GameObject targetObjectB; // Ŀ������B
    public Transform endTransform; // ����λ�õ�Transform
    public string targetTag; // Ҫ�����ض�Tag
    public TMP_Text warningText; // ��ʾ�ı���TMP���
    public float moveDuration = 1f; // �ƶ��ĳ���ʱ��
    public float warningDuration = 2f; // ��ʾ�ı���ʾ�ĳ���ʱ��

    void Start()
    {
        // ��ʼ��ʱ������ʾ�ı�
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }

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
        // ���targetObjectB�Ƿ����TagΪ��Wrapper�����Ӷ���
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
            // ���û���ҵ���Wrapper���Ӷ�����ʾ���沢��
            ShowWarning("Need to Select Wrapper first");
            return;
        }

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

        // ��������A��λ��������B�ֲ�����ϵ�е�λ��
        Vector3 startLocalPosition = targetObjectB.transform.InverseTransformPoint(this.transform.position);

        // ʵ����Prefab
        GameObject newChild = Instantiate(prefabToInstantiate);

        // ���ø�����
        newChild.transform.SetParent(targetObjectB.transform, false);

        // ������������ľֲ�����λ��Ϊ����A�Ķ�Ӧλ��
        newChild.transform.localPosition = startLocalPosition;

        // ʹ��DoTween��Prefab����ʼλ���ƶ���endTransformָ����λ��
        newChild.transform.DOLocalMove(targetObjectB.transform.InverseTransformPoint(endTransform.position), moveDuration).SetEase(Ease.OutQuad);
        newChild.transform.DORotate(new Vector3(360, 0, 0), moveDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
    }

    void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningText.gameObject.SetActive(true);

            // ����ʾ�ı�ͨ��DoTween��
            warningText.rectTransform.DOShakePosition(0.5f, new Vector3(5f, 5f, 0), 10, 90, false, true);

            // ����2����Զ��ر���ʾ
            DOVirtual.DelayedCall(warningDuration, () => warningText.gameObject.SetActive(false));
        }
    }
}
