using System.Collections.Generic;
using UnityEngine;

public class HandCardManager : MonoBehaviour
{
    public static HandCardManager Instance { get; private set; }

    [Header("Card Data and Prefab")]
    public List<CardData> cardDataList;
    public GameObject cardPrefab; // 卡牌预制体

    public CardArrangement cardArrangement; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RandomGenerateCard()
    {
        if (cardDataList.Count == 0 || cardPrefab == null || cardArrangement == null)
        {
            Debug.LogError("CardDataList is empty, CardPrefab is missing, or CardFanArrangement is not assigned!");
            return;
        }

        // 随机选择一个CardData
        CardData randomCardData = cardDataList[Random.Range(0, cardDataList.Count)];

        // 使用 CardFanArrangement 的 AddCard 方法生成卡牌
        GameObject cardObject = Instantiate(cardPrefab);
        CardBehaviour cardBehaviour = cardObject.GetComponent<CardBehaviour>();
        // 使用 CardArrangement 的 AddCard 方法生成卡牌
        cardArrangement.EnqueueCard(cardPrefab, randomCardData); // 通过 AddCard 添加并排列卡牌
    }

    public void GenerateSpecificCard(CardData cardData)
    {
        if (cardData == null || cardPrefab == null || cardArrangement == null)
        {
            Debug.LogError("CardData is null, CardPrefab is missing, or CardArrangement is not assigned!");
            return;
        }

        // 创建一个新的生成请求并加入队列
        cardArrangement.EnqueueCard(cardPrefab, cardData);
        Debug.Log($"Enqueued specific card: {cardData.CardName}");
    }
}

[System.Serializable]
public class CardSpawnRequest
{
    public GameObject cardPrefab;
    public CardData cardData;

    public CardSpawnRequest(GameObject prefab, CardData data)
    {
        cardPrefab = prefab;
        cardData = data;
    }
}
