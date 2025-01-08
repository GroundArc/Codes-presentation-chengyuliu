using UnityEngine;

public class TubeInteraction : MonoBehaviour
{
    public Item item;
    public Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void OnMouseOver()
    {
        // �������ͣ��Collider��ʱ����Outline
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    void OnMouseExit()
    {
        // ������뿪Colliderʱ����Outline
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void OnMouseDown()
    {
        // �������ʱ�Ƴ�Item�����ٶ���
        ColorOwned.instance.Remove(item);
        Destroy(gameObject);
    }
}
