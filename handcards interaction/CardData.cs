using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card System/Card Data", order = 1)]
public class CardData : ScriptableObject
{
    public enum CardType
    {
        DefaultSkill,  // 默认技能
        SpecialSkill   // 特殊技能
    }

    [Header("基本信息")]
    [SerializeField] private int cardId;
    [SerializeField] private string cardName;
    [SerializeField] private CardType cardType;
    [SerializeField] private int actionCost;
    [SerializeField] private Sprite skillIcon;
    [TextArea(2, 5)]
    [SerializeField] private string description;

    public int CardId => cardId;
    public string CardName => cardName;
    public CardType Type => cardType;
    public int ActionCost => actionCost;
    public Sprite SkillIcon => skillIcon;
    public string Description => description;
}
