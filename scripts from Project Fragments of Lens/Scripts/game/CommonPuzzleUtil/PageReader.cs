using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

public class PageReader : MonoBehaviour
{
    [Header("Page Settings")]
    public List<RectTransform> pages; // 存储所有页面的列表
    public Vector3 storagePosition; // 页面存储位置
    public Vector3 displayPosition; // 页面显示位置
    public Vector3 recyclePosition; // 页面回收位置

    [Header("Tween Settings")]
    public float moveDuration = 1f; // 页面移动时长
    public Ease easeType = Ease.OutCubic; // 缓动类型

    [Header("UI Elements")]
    public TextMeshProUGUI pageText; // 显示当前页数和总页数的TextMeshPro组件
    public int currentPageIndex = -1; // 当前页的索引，初始化为 -1 表示没有页显示

    private int totalPages; // 总页数

    private void Start()
    {
        totalPages = pages.Count;

        // 初始化所有页面为存储位置
        foreach (var page in pages)
        {
            page.localPosition = storagePosition;
        }

        UpdatePageText(); // 初始更新页码，显示0页
    }

    // 结束阅读，将所有页面移动到初始位置并重置页码显示
    public void EndReading()
    {
        for (int i = 0; i < totalPages; i++)
        {
            // 把每一页移动到回收位置并瞬移到存储位置
            pages[i].DOLocalMove(recyclePosition, moveDuration).SetEase(easeType).OnComplete(() =>
            {
                pages[i].localPosition = storagePosition;
            });
        }

        currentPageIndex = -1; // 重置页数为 0，表示没有页显示
        UpdatePageText();
    }

    // 显示当前页，处理页面切换
    private void ShowPage(int pageIndex)
    {
        if (pageIndex != currentPageIndex)
        {
            // 如果没有页显示并且要显示第一页
            if (currentPageIndex == -1 && pageIndex == 0)
            {
                // 直接显示第一页
                pages[pageIndex].DOLocalMove(displayPosition, moveDuration).SetEase(easeType);
            }
            else if (currentPageIndex != -1)
            {
                // 把当前页移动到回收位置并瞬间返回存储位置
                pages[currentPageIndex].DOLocalMove(recyclePosition, moveDuration).SetEase(easeType).OnComplete(() =>
                {
                    pages[currentPageIndex].localPosition = storagePosition; // 瞬间返回存储位置
                });

                // 切换到新页面并移动到显示位置
                pages[pageIndex].DOLocalMove(displayPosition, moveDuration).SetEase(easeType);
            }

            currentPageIndex = pageIndex; // 更新当前页索引
            UpdatePageText(); // 更新页码显示
        }
    }

    // 更新页码显示
    private void UpdatePageText()
    {
        if (currentPageIndex == -1)
        {
            pageText.text = $"0/{totalPages}"; // 显示 0 页
        }
        else
        {
            pageText.text = $"{currentPageIndex + 1}/{totalPages}";
        }
    }

    // 显示下一页
    public void ShowNextPage()
    {
        if (currentPageIndex < totalPages - 1)
        {
            ShowPage(currentPageIndex + 1); // 切换到下一页
        }
    }

    // 显示上一页的逻辑
    public void ShowPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            // 当前页滑动回存储位置
            pages[currentPageIndex].DOLocalMove(storagePosition, moveDuration).SetEase(easeType);

            // 上一页瞬间移动到回收位置
            pages[currentPageIndex - 1].localPosition = recyclePosition;

            // 然后平滑地从回收位置移动到显示位置
            pages[currentPageIndex - 1].DOLocalMove(displayPosition, moveDuration).SetEase(easeType);

            currentPageIndex--; // 更新当前页索引
            UpdatePageText(); // 更新页码显示
        }
    }

    // 协程延迟执行 ShowNextPage
    public IEnumerator DelayShowNextPage(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定时间
        ShowNextPage(); // 延迟后显示下一页
    }

    // 调用协程延迟执行方法
    public void StartDelayedShowNextPage(float delay = 1.5f)
    {
        StartCoroutine(DelayShowNextPage(delay));
    }
}
