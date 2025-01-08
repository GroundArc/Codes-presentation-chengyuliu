using System.Collections.Generic;
using UnityEngine;

public class HandCardManager : MonoBehaviour
{
    public static HandCardManager Instance { get; private set; }

    [Header("Card Data and Prefab")]
    public List<CardData> cardDataList;
    public GameObject cardPrefab; // ����Ԥ����

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

        // ���ѡ��һ��CardData
        CardData randomCardData = cardDataList[Random.Range(0, cardDataList.Count)];

        // ʹ�� CardFanArrangement �� AddCard �������ɿ���
        GameObject cardObject = Instantiate(cardPrefab);
        CardBehaviour cardBehaviour = cardObject.GetComponent<CardBehaviour>();
        // ʹ�� CardArrangement �� AddCard �������ɿ���
        cardArrangement.EnqueueCard(cardPrefab, randomCardData); // ͨ�� AddCard ��Ӳ����п���
    }

    public void GenerateSpecificCard(CardData cardData)
    {
        if (cardData == null || cardPrefab == null || cardArrangement == null)
        {
            Debug.LogError("CardData is null, CardPrefab is missing, or CardArrangement is not assigned!");
            return;
        }

        // ����һ���µ��������󲢼������
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
