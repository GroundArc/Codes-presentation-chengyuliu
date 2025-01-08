using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardArrangement : MonoBehaviour
{
    public static CardArrangement Instance { get; private set; }

    [Header("Fan Arrangement Settings")]
    public float base_card_spacing = 100f;
    public float base_card_angle = 10f;
    public float card_offset_y = 10f;

    [Header("Card Animation Settings")]
    public Vector3 card_entry_position = new Vector3(-500f, -500f, 0f);
    public Vector3 card_center_position = new Vector3(0f, -200f, 50f);
    public float entry_animation_duration = 1.0f;
    public float center_pause_duration = 0.5f;

    public CardPreviewPanel preview;
    public GameObject discardButton; // ���ư�ť
    private Dictionary<int, Transform> cardMap = new Dictionary<int, Transform>();
    private Dictionary<int, Vector2> cardOriginalPositions = new Dictionary<int, Vector2>();
    private Queue<CardSpawnRequest> cardSpawnQueue = new Queue<CardSpawnRequest>();
    private bool isProcessingQueue = false; // �Ƿ����ڴ�����еı��

    private int currentlyHoveredCardIndex = -1;
    private int selectedCardIndex = -1; // ��ǰѡ�еĿ�������
    public bool isAnimating = false;
    private int nextCardIndex = 1;

    private void Update()
    {
        // �������Ҽ�����
        if (Input.GetMouseButtonDown(1)) //����Ҽ�
        {
            if (selectedCardIndex != -1)
            {
                Debug.Log("Right-click detected. Deselecting selected card.");
                DeselectCard();
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        if (discardButton != null)
        {
            discardButton.SetActive(false); // ��ʼ�������ư�ť
        }
    }

    private void ArrangeCards()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // ���̶ֹ���ˮƽ���
        float dynamic_spacing = base_card_spacing; // ˮƽ������ֲ���
        float dynamic_angle = Mathf.Max(1, base_card_angle / childCount); // ���ݿ������������Ƕ�
        float dynamic_y_offset_factor = Mathf.Max(1, 1f / childCount); // ���ݿ���������СYƫ�Ʊ���

        int index = 0;
        float count_half = (childCount - 1) / 2f;

        foreach (Transform card in transform)
        {
            int cardIndex = GetCardIndexByTransform(card);
            if (cardIndex == -1) continue;

            // ����ÿ�ſ��Ƶ�ƫ��
            float xOffset = (index - count_half) * dynamic_spacing;
            float yOffset = (index - count_half) * (index - count_half) * -card_offset_y * dynamic_y_offset_factor; // ��ֱƫ�Ƽ���
            float angle = (index - count_half) * -dynamic_angle;

            RectTransform rect = card.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.DOKill(); // ��ֹδ��ɵĶ���
                Vector2 finalPos = new Vector2(xOffset, yOffset);

                // ʹ�� DOTween ��������λ�ú���ת
                rect.DOAnchorPos(finalPos, 0.5f).SetEase(Ease.OutQuad);
                rect.DOLocalRotate(new Vector3(0f, 0f, angle), 0.5f).SetEase(Ease.OutQuad);

                // ���¿��Ƶ�ԭʼλ��
                cardOriginalPositions[cardIndex] = finalPos;
            }

            index++;
        }
    }

    /// <summary>
    /// �ⲿ���ø÷������Ŷӵȴ�����һ�ſ�
    /// </summary>
    public void EnqueueCard(GameObject cardPrefab, CardData cardData)
    {
        // ����������������
        cardSpawnQueue.Enqueue(new CardSpawnRequest(cardPrefab, cardData));

        // �����ǰû���ڴ�����У�������Э��
        if (!isProcessingQueue)
        {
            StartCoroutine(ProcessCardSpawnQueue());
        }
    }

    /// <summary>
    /// Э�̣�����ȡ�������е���������ÿ�ζ��ȴ�������ɺ��ټ���
    /// </summary>
    private IEnumerator ProcessCardSpawnQueue()
    {
        isProcessingQueue = true;

        while (cardSpawnQueue.Count > 0)
        {
            CardSpawnRequest request = cardSpawnQueue.Dequeue();

            // ����һ����ʵ����Ӳ����Ŷ������ķ���
            yield return StartCoroutine(SpawnCardWithAnimation(request.cardPrefab, request.cardData));
        }

        isProcessingQueue = false;
    }

    /// <summary>
    /// ʵ�����ɿ��ơ����ȴ�����������ɵ�Э��
    /// </summary>
    private IEnumerator SpawnCardWithAnimation(GameObject cardPrefab, CardData cardData)
    {
        // ��Ƕ���������
        isAnimating = true;

        GameObject card = Instantiate(cardPrefab, transform);
        int cardIndex = nextCardIndex++;
        cardMap[cardIndex] = card.transform;

        CardBehaviour cardBehaviour = card.GetComponent<CardBehaviour>();
        if (cardBehaviour != null)
        {
            cardBehaviour.SetCardInfo(cardData, cardIndex);
        }

        RectTransform rect = card.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition3D = card_entry_position;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(rect.DOAnchorPos3D(card_center_position, entry_animation_duration).SetEase(Ease.OutCubic))
                    .Join(rect.DOScale(Vector3.one * 1.5f, entry_animation_duration).SetEase(Ease.OutCubic))
                    .AppendInterval(center_pause_duration)
                    .Append(rect.DOAnchorPos3D(Vector3.zero, entry_animation_duration).SetEase(Ease.InCubic))
                    .Join(rect.DOScale(Vector3.one, entry_animation_duration).SetEase(Ease.InCubic))
                    .OnComplete(() =>
                    {
                        // ��������ɺ�ArrangeCards������Ƕ�������
                        ArrangeCards();
                        isAnimating = false;
                        Debug.Log($"AddCard complete. CardIndex: {cardIndex}, CardName: {cardData.CardName}");
                    });

            while (isAnimating)
            {
                yield return null;
            }
        }
    }


    public void AddCard(GameObject cardPrefab, CardData cardData)
    {
        if (isAnimating)
        {
            Debug.Log("Animation in progress, cannot add card.");
            return;
        }

        isAnimating = true;

        GameObject card = Instantiate(cardPrefab, transform);
        int cardIndex = nextCardIndex++;
        cardMap[cardIndex] = card.transform;

        CardBehaviour cardBehaviour = card.GetComponent<CardBehaviour>();
        if (cardBehaviour != null)
        {
            cardBehaviour.SetCardInfo(cardData, cardIndex);
        }

        RectTransform rect = card.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition3D = card_entry_position;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(rect.DOAnchorPos3D(card_center_position, entry_animation_duration).SetEase(Ease.OutCubic))
                    .Join(rect.DOScale(Vector3.one * 1.5f, entry_animation_duration).SetEase(Ease.OutCubic))
                    .AppendInterval(center_pause_duration)
                    .Append(rect.DOAnchorPos3D(Vector3.zero, entry_animation_duration).SetEase(Ease.InCubic))
                    .Join(rect.DOScale(Vector3.one, entry_animation_duration).SetEase(Ease.InCubic))
                    .OnComplete(() =>
                    {
                        ArrangeCards();
                        isAnimating = false;
                        Debug.Log($"AddCard complete. CardIndex: {cardIndex}, CardName: {cardData.CardName}");
                    });
        }
    }

    public void RemoveCard(Transform card)
    {
        if (isAnimating)
        {
            Debug.Log("Animation in progress, cannot remove card.");
            return;
        }

        int cardIndexToRemove = -1;
        foreach (var pair in cardMap)
        {
            if (pair.Value == card)
            {
                cardIndexToRemove = pair.Key;
                break;
            }
        }

        if (cardIndexToRemove != -1)
        {
            cardMap.Remove(cardIndexToRemove);
            cardOriginalPositions.Remove(cardIndexToRemove);
            Destroy(card.gameObject);
            ArrangeCards();
            Debug.Log("Removed card: " + cardIndexToRemove + ", Current hovered card: " + currentlyHoveredCardIndex);
        }
    }

    public void SelectCard(int cardIndex)
    {
        if (selectedCardIndex == cardIndex)
        {
            Debug.Log($"Card {cardIndex} is already selected.");
            return;
        }

        // �������ѡ�еĿ��ƣ�ȡ����ѡ��״̬
        if (selectedCardIndex != -1)
        {
            DeselectCard();
        }

        selectedCardIndex = cardIndex;

        if (cardMap.TryGetValue(cardIndex, out Transform card))
        {
            RectTransform rect = card.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.DOKill();
                Vector2 hoverPos = new Vector2(cardOriginalPositions[cardIndex].x, cardOriginalPositions[cardIndex].y + 50f);

                rect.DOAnchorPos(hoverPos, 0.3f).SetEase(Ease.OutQuad);
                Debug.Log($"Card {cardIndex} selected.");
            }
        }
        ShowCardPreview(cardIndex); // ��ʾ����Ԥ��
                                    // ��ʾ���ư�ť���󶨵���¼�
        if (discardButton != null)
        {
            discardButton.SetActive(true);
            discardButton.GetComponent<Button>().onClick.RemoveAllListeners();
            discardButton.GetComponent<Button>().onClick.AddListener(() => DiscardCard(cardIndex));
        }
    }

    public void DiscardCard(int cardIndex)
    {
        if (!cardMap.ContainsKey(cardIndex))
        {
            Debug.LogWarning($"Card {cardIndex} not found for discard.");
            return;
        }
        preview.HideCardInfo();
        // ɾ�����Ƶ� GameObject
        Transform card = cardMap[cardIndex];
        Destroy(card.gameObject);

        // ���ֵ����Ƴ���Ӧ����
        cardMap.Remove(cardIndex);
        cardOriginalPositions.Remove(cardIndex);

        // ���·�������
        Dictionary<int, Transform> updatedCardMap = new Dictionary<int, Transform>();
        Dictionary<int, Vector2> updatedCardOriginalPositions = new Dictionary<int, Vector2>();

        int newIndex = 1;
        foreach (var pair in cardMap)
        {
            updatedCardMap[newIndex] = pair.Value;
            updatedCardOriginalPositions[newIndex] = cardOriginalPositions[pair.Key];

            // ���¿��Ƶ�����
            var cardBehaviour = pair.Value.GetComponent<CardBehaviour>();
            if (cardBehaviour != null)
            {
                cardBehaviour.UpdateCardIndex(newIndex);
            }

            newIndex++;
        }

        cardMap = updatedCardMap;
        cardOriginalPositions = updatedCardOriginalPositions;

        selectedCardIndex = -1;

        Debug.Log($"Card {cardIndex} discarded and indices reassigned.");

        // �������ư�ť���������п���
        if (discardButton != null)
        {
            discardButton.SetActive(false);
        }
        ArrangeCards();
    }

    public void DeselectCard()
    {
        if (selectedCardIndex == -1) return;

        if (cardMap.TryGetValue(selectedCardIndex, out Transform card))
        {
            RectTransform rect = card.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.DOKill();
                rect.DOAnchorPos(cardOriginalPositions[selectedCardIndex], 0.3f).SetEase(Ease.OutQuad);
                Debug.Log($"Card {selectedCardIndex} deselected.");
            }
        }
        preview.HideCardInfo();
        selectedCardIndex = -1;
        // �������ư�ť
        if (discardButton != null)
        {
            discardButton.SetActive(false);
        }
    }

    public void HoverCard(int cardIndex)
    {
        if (isAnimating || selectedCardIndex != -1)
        {
            Debug.Log("Animation in progress, cannot hover card.");
            return;
        }

        UnhoverAllCardsExcept(cardIndex);

        if (cardMap.ContainsKey(cardIndex))
        {
            Transform card = cardMap[cardIndex];
            RectTransform rect = card.GetComponent<RectTransform>();

            if (rect != null)
            {
                rect.DOKill();
                Vector2 originalPos = cardOriginalPositions[cardIndex];
                Vector2 hoverPos = new Vector2(originalPos.x, originalPos.y + 50f);

                Debug.Log("Start hovering card: " + cardIndex + " from " + originalPos + " to " + hoverPos);

                rect.DOAnchorPos(hoverPos, 0.3f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        currentlyHoveredCardIndex = cardIndex;
                        ShowCardPreview(cardIndex); // ��ʾ����Ԥ��
                    });
            }
        }
    }

    private void ShowCardPreview(int cardIndex)
    {
        if (!cardMap.ContainsKey(cardIndex))
        {
            Debug.LogWarning($"Card index {cardIndex} not found in cardMap.");
            return;
        }

        Transform card = cardMap[cardIndex];
        CardBehaviour cardBehaviour = card.GetComponent<CardBehaviour>();
        if (cardBehaviour != null)
        {
            CardData cardData = cardBehaviour.GetCardData();
            if (cardData != null)
            {
                preview.ShowCardInfo(cardData); // ����Ԥ�������ʾ��Ϣ
                Debug.Log($"Card preview displayed: {cardIndex} - {cardData.CardName}");
            }
            else
            {
                Debug.LogWarning($"CardData is null for card index {cardIndex}.");
            }
        }
        else
        {
            Debug.LogWarning($"CardBehaviour not found on card index {cardIndex}.");
        }
    }

    public void UnhoverCard(int cardIndex)
    {
        if (selectedCardIndex == cardIndex) return;
        if (!cardMap.ContainsKey(cardIndex))
        {
            Debug.Log("Card " + cardIndex + " not found in cardMap to unhover.");
            return;
        }

        Transform card = cardMap[cardIndex];
        RectTransform rect = card.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.DOKill();
            Vector2 originalPos = cardOriginalPositions.ContainsKey(cardIndex) ? cardOriginalPositions[cardIndex] : Vector2.zero;
            Debug.Log("Unhovering card: " + cardIndex + " back to " + originalPos);

            rect.DOAnchorPos(originalPos, 0.3f).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    if (currentlyHoveredCardIndex == cardIndex)
                    {
                        currentlyHoveredCardIndex = -1;
                    }
                   preview.HideCardInfo();
                    Debug.Log("Card " + cardIndex + " unhovered.");
                });
        }
    }

    private void UnhoverAllCardsExcept(int cardIndex)
    {
        foreach (var pair in cardMap)
        {
            int otherIndex = pair.Key;
            if (otherIndex != cardIndex && currentlyHoveredCardIndex == otherIndex)
            {
                UnhoverCard(otherIndex);
            }
        }
        preview.HideCardInfo();
    }

    private int GetCardIndexByTransform(Transform cardTransform)
    {
        foreach (var pair in cardMap)
        {
            if (pair.Value == cardTransform)
                return pair.Key;
        }
        return -1;
    }

    private void Start()
    {
        ArrangeCards();
        
    }
}
