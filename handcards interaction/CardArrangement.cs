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
    public GameObject discardButton; // 弃牌按钮
    private Dictionary<int, Transform> cardMap = new Dictionary<int, Transform>();
    private Dictionary<int, Vector2> cardOriginalPositions = new Dictionary<int, Vector2>();
    private Queue<CardSpawnRequest> cardSpawnQueue = new Queue<CardSpawnRequest>();
    private bool isProcessingQueue = false; // 是否正在处理队列的标记

    private int currentlyHoveredCardIndex = -1;
    private int selectedCardIndex = -1; // 当前选中的卡牌索引
    public bool isAnimating = false;
    private int nextCardIndex = 1;

    private void Update()
    {
        // 检测鼠标右键按下
        if (Input.GetMouseButtonDown(1)) //鼠标右键
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
            discardButton.SetActive(false); // 初始隐藏弃牌按钮
        }
    }

    private void ArrangeCards()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // 保持固定的水平间隔
        float dynamic_spacing = base_card_spacing; // 水平间隔保持不变
        float dynamic_angle = Mathf.Max(1, base_card_angle / childCount); // 根据卡牌数量调整角度
        float dynamic_y_offset_factor = Mathf.Max(1, 1f / childCount); // 根据卡牌数量缩小Y偏移比例

        int index = 0;
        float count_half = (childCount - 1) / 2f;

        foreach (Transform card in transform)
        {
            int cardIndex = GetCardIndexByTransform(card);
            if (cardIndex == -1) continue;

            // 计算每张卡牌的偏移
            float xOffset = (index - count_half) * dynamic_spacing;
            float yOffset = (index - count_half) * (index - count_half) * -card_offset_y * dynamic_y_offset_factor; // 垂直偏移减少
            float angle = (index - count_half) * -dynamic_angle;

            RectTransform rect = card.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.DOKill(); // 终止未完成的动画
                Vector2 finalPos = new Vector2(xOffset, yOffset);

                // 使用 DOTween 动画更新位置和旋转
                rect.DOAnchorPos(finalPos, 0.5f).SetEase(Ease.OutQuad);
                rect.DOLocalRotate(new Vector3(0f, 0f, angle), 0.5f).SetEase(Ease.OutQuad);

                // 更新卡牌的原始位置
                cardOriginalPositions[cardIndex] = finalPos;
            }

            index++;
        }
    }

    /// <summary>
    /// 外部调用该方法，排队等待生成一张卡
    /// </summary>
    public void EnqueueCard(GameObject cardPrefab, CardData cardData)
    {
        // 将生成请求加入队列
        cardSpawnQueue.Enqueue(new CardSpawnRequest(cardPrefab, cardData));

        // 如果当前没有在处理队列，则启动协程
        if (!isProcessingQueue)
        {
            StartCoroutine(ProcessCardSpawnQueue());
        }
    }

    /// <summary>
    /// 协程：依次取出队列中的生成请求，每次都等待动画完成后再继续
    /// </summary>
    private IEnumerator ProcessCardSpawnQueue()
    {
        isProcessingQueue = true;

        while (cardSpawnQueue.Count > 0)
        {
            CardSpawnRequest request = cardSpawnQueue.Dequeue();

            // 调用一个“实际添加并播放动画”的方法
            yield return StartCoroutine(SpawnCardWithAnimation(request.cardPrefab, request.cardData));
        }

        isProcessingQueue = false;
    }

    /// <summary>
    /// 实际生成卡牌、并等待动画播放完成的协程
    /// </summary>
    private IEnumerator SpawnCardWithAnimation(GameObject cardPrefab, CardData cardData)
    {
        // 标记动画进行中
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
                        // 当动画完成后，ArrangeCards，并标记动画结束
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

        // 如果已有选中的卡牌，取消其选中状态
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
        ShowCardPreview(cardIndex); // 显示卡牌预览
                                    // 显示弃牌按钮并绑定点击事件
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
        // 删除卡牌的 GameObject
        Transform card = cardMap[cardIndex];
        Destroy(card.gameObject);

        // 从字典中移除对应数据
        cardMap.Remove(cardIndex);
        cardOriginalPositions.Remove(cardIndex);

        // 重新分配索引
        Dictionary<int, Transform> updatedCardMap = new Dictionary<int, Transform>();
        Dictionary<int, Vector2> updatedCardOriginalPositions = new Dictionary<int, Vector2>();

        int newIndex = 1;
        foreach (var pair in cardMap)
        {
            updatedCardMap[newIndex] = pair.Value;
            updatedCardOriginalPositions[newIndex] = cardOriginalPositions[pair.Key];

            // 更新卡牌的索引
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

        // 隐藏弃牌按钮并重新排列卡牌
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
        // 隐藏弃牌按钮
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
                        ShowCardPreview(cardIndex); // 显示卡牌预览
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
                preview.ShowCardInfo(cardData); // 调用预览面板显示信息
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
