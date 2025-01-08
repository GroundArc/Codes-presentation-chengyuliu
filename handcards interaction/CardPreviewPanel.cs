using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CardPreviewPanel : MonoBehaviour
{
    public static CardPreviewPanel Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject hoverPanel;   // ������Ϣ���
    public TMP_Text cardNameText;   // ��������
    public TMP_Text cardDescriptionText; // ��������
    public TMP_Text cardValueText;  // ������ֵ
    public Image cardImage;         // ����ͼƬ

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        hoverPanel.SetActive(false); // Ĭ���������
    }

    public void ShowCardInfo(CardData cardData)
    {
        if (cardData == null)
        {
            Debug.LogWarning("CardData is null, cannot display hover panel!");
            return;
        }

        Debug.Log("Displaying card info: " + cardData.CardName);

        cardNameText.text = cardData.CardName;
        cardDescriptionText.text = cardData.Description;
        cardValueText.text = cardData.ActionCost.ToString();
        cardImage.sprite = cardData.SkillIcon;

        hoverPanel.SetActive(true);
    }

    public void HideCardInfo()
    {
        Debug.Log("Hiding card info panel.");
        hoverPanel.SetActive(false);
    }

}
