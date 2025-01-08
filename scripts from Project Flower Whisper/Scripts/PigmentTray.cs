using System.Collections.Generic;
using UnityEngine;

public class PigmentTray : MonoBehaviour
{
    public ColorOwned colorOwned; // ��ɫ������
    public Collider trayArea; // ���ڶ��������������ײ��
    public int columns = 3; // ÿ�е����Ͽ�����

    private List<GameObject> displayedPigments = new List<GameObject>();

    void Start()
    {
        if (colorOwned == null)
        {
            Debug.LogError("ColorOwned is not assigned.");
            return;
        }

        if (trayArea == null)
        {
            Debug.LogError("Tray area is not assigned.");
            return;
        }

        DisplayPigments();
        colorOwned.onItemChangedCallBack += DisplayPigments; // ������Ʒ�仯�¼�
    }

    void OnDestroy()
    {
        colorOwned.onItemChangedCallBack -= DisplayPigments; // ȡ��������Ʒ�仯�¼�
    }

    // ��������ʾ���Ͽ�
    public void DisplayPigments()
    {
        ClearDisplayedPigments();

        List<ItemStack> items = colorOwned.GetItems();
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("No available pigments to display.");
            return;
        }

        Bounds bounds = trayArea.bounds;
        float spacingX = bounds.size.x / columns;
        int rows = Mathf.CeilToInt(items.Count / (float)columns);
        float spacingZ = bounds.size.z / rows;
        float yPosition = bounds.center.y; // ʹ����ײ��������Yֵ

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].quantity > 0 && items[i].item.pigmentPrefab != null)
            {
                int row = i / columns;
                int col = i % columns;

                Vector3 position = new Vector3(
                    bounds.min.x + col * spacingX + spacingX / 2,
                    yPosition,
                    bounds.min.z + row * spacingZ + spacingZ / 2
                );

                GameObject pigmentInstance = Instantiate(items[i].item.pigmentPrefab, position, Quaternion.identity);
                pigmentInstance.transform.SetParent(trayArea.transform, true);
                pigmentInstance.transform.localPosition = position - trayArea.transform.position; // ȷ��λ����ȷ
                pigmentInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // ��������Ϊ0.5
                pigmentInstance.GetComponent<PigmentDisplay>().SetQuantity(items[i].quantity);
                pigmentInstance.GetComponent<DragPigment>().initialPosition = pigmentInstance.transform.position; // ���ó�ʼλ��
                pigmentInstance.GetComponent<DragPigment>().initialParent = trayArea.transform; // ���ó�ʼ������
                pigmentInstance.GetComponent<DragPigment>().pigmentItem = items[i].item; // �������Ͽ��Ӧ��Item
                displayedPigments.Add(pigmentInstance);
            }
        }
    }

    // �����������ʾ�����Ͽ�
    public void ClearDisplayedPigments()
    {
        foreach (GameObject pigment in displayedPigments)
        {
            Destroy(pigment);
        }
        displayedPigments.Clear();
    }
}
