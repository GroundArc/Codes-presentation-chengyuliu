using UnityEngine;

public class GridItem : MonoBehaviour
{
    public Vector2 Id; // ���� ID
    public GameObject content; // �洢�����ϵ���Ʒ
    public ItemType contentType = ItemType.None; // �洢������Ʒ������

    // ���ø��� ID
    public void SetId(Vector2 Id)
    {
        this.Id = Id;
    }

    // ������Ʒ��������
    public void PlaceContent(GameObject obj, ItemType type)
    {
        if (content != null)
        {
            Debug.LogWarning($"Grid ({Id.x}, {Id.y}) already contains an item of type {contentType}.");
            return;
        }

        content = obj;
        contentType = type;

        // ������Ʒ��λ����Ϣ
        if (content != null)
        {
            content.transform.position = GetTransPos();
        }
    }

    // �Ƴ������ϵ���Ʒ
    public void RemoveContent()
    {
        content = null;
        contentType = ItemType.None;
    }

    // ��ȡ���������������е�λ��
    public Vector3 GetTransPos()
    {
        return this.transform.position + new Vector3(0, 1.2f, 0); // ƫ��һ���߶�
    }
}

// ��Ʒ�����ö��
public enum ItemType
{
    None,   // �ո���
    Player, // ���
    Monster, // ����
    Trap,   // ����
    Chest   // ����
}
