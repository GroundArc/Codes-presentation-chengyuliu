using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Card UI Components")]
    public Image skillIconImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI actionCostText;
    public string description;
    public int actionCost;
    public CardData.CardType Type;

    private CardData cardData;
    private int cardIndex;
    private bool isHovered = false;
    private bool isSelected = false;

    /// <summary>
    /// 初始化卡牌数据
    /// </summary>
    /// <param name="data">卡牌数据</param>
    /// <param name="index">卡牌索引</param>
    public void SetCardInfo(CardData data, int index)
    {
        cardData = data;
        cardIndex = index;

        if (cardData != null)
        {
            UpdateCardUI();
            actionCost = cardData.ActionCost;
            Type = cardData.Type;
        }
        else
        {
            Debug.LogWarning("CardData is null. Cannot initialize card.");
        }
    }

    /// <summary>
    /// 更新卡牌UI显示
    /// </summary>
    private void UpdateCardUI()
    {
        if (cardData == null) return;

        skillIconImage.sprite = cardData.SkillIcon;
        cardNameText.text = cardData.CardName;
        actionCostText.text = cardData.ActionCost.ToString();
        description = cardData.Description;
    }
    public void UpdateCardIndex(int newIndex)
    {
        cardIndex = newIndex;
        Debug.Log($"Card index updated to {newIndex}.");
    }


    public CardData GetCardData()
    {
        return cardData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected || CardArrangement.Instance == null || CardArrangement.Instance.isAnimating) return;

        if (!isHovered)
        {
            isHovered = true;
            CardArrangement.Instance.HoverCard(cardIndex);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardArrangement.Instance == null) return;

        if (isHovered)
        {
            isHovered = false;
            CardArrangement.Instance.UnhoverCard(cardIndex);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            Debug.Log($"Card Selected: {cardIndex} - {cardData.CardName}");
            CardArrangement.Instance.SelectCard(cardIndex); // 通知 CardArrangement 锁定该卡牌
               
            
        }
        
    }

   
}
