using UnityEngine;
using TMPro;

public class ItemDerive : Interactable
{
    public Item item; // �����ScriptableObject
    private TMP_Text tooltipText; // ������ʾItem���Ƶ�TMP���

    void Awake()
    {
        // ��Canvas�в�����Ϊ"ColorDerive"����Ϸ���󲢻�ȡ��TMP_Text���
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
            // ��ʲôû���
        }
    }

    void OnMouseEnter()
    {
        if (tooltipText != null)
        {
            tooltipText.text = "Can Collect: " + item.name;
            tooltipText.color = item.renderColor; // ����������ɫΪItem��renderColor
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
