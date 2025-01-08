using UnityEngine;
using TMPro;

public class ItemDerive : Interactable
{
    public Item item; // 这个是ScriptableObject
    private TMP_Text tooltipText; // 用于显示Item名称的TMP组件

    void Awake()
    {
        // 在Canvas中查找名为"ColorDerive"的游戏对象并获取其TMP_Text组件
        GameObject colorDeriveObject = GameObject.Find("ColorDerive");
        if (colorDeriveObject != null)
        {
            tooltipText = colorDeriveObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("No GameObject named 'ColorDerive' found in the scene.");
        }
    }

    public override void Interact()
    {
        base.Interact();
        Derive();
    }

    void Derive()
    {
        Debug.Log("Derive object's color: " + item.name);
        bool wasDerived = Backpack.instance.Add(item);
        // Add to Color owned
        if (wasDerived)
        {
            // 做什么没想好
        }
    }

    void OnMouseEnter()
    {
        if (tooltipText != null)
        {
            tooltipText.text = "Can Collect: " + item.name;
            tooltipText.color = item.renderColor; // 设置字体颜色为Item的renderColor
            tooltipText.gameObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (tooltipText != null)
        {
            tooltipText.gameObject.SetActive(false);
        }
    }
}
