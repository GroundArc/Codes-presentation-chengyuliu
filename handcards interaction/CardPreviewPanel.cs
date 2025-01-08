using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CardPreviewPanel : MonoBehaviour
{
    public static CardPreviewPanel Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject hoverPanel;   // ¿¨ÅÆÐÅÏ¢Ãæ°å
    public TMP_Text cardNameText;   // ¿¨ÅÆÃû×Ö
    public TMP_Text cardDescriptionText; // ¿¨ÅÆÃèÊö
    public TMP_Text cardValueText;  // ¿¨ÅÆÊýÖµ
    public Image cardImage;         // ¿¨ÅÆÍ¼Æ¬

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        hoverPanel.SetActive(false); // Ä¬ÈÏÒþ²ØÃæ°å
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
