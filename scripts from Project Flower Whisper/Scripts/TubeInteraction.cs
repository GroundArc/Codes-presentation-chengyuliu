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
        // 当鼠标悬停在Collider上时启用Outline
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    void OnMouseExit()
    {
        // 当鼠标离开Collider时禁用Outline
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void OnMouseDown()
    {
        // 当鼠标点击时移除Item并销毁对象
        ColorOwned.instance.Remove(item);
        Destroy(gameObject);
    }
}
