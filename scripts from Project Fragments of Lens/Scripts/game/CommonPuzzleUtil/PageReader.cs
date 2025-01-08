using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

public class PageReader : MonoBehaviour
{
    [Header("Page Settings")]
    public List<RectTransform> pages; // �洢����ҳ����б�
    public Vector3 storagePosition; // ҳ��洢λ��
    public Vector3 displayPosition; // ҳ����ʾλ��
    public Vector3 recyclePosition; // ҳ�����λ��

    [Header("Tween Settings")]
    public float moveDuration = 1f; // ҳ���ƶ�ʱ��
    public Ease easeType = Ease.OutCubic; // ��������

    [Header("UI Elements")]
    public TextMeshProUGUI pageText; // ��ʾ��ǰҳ������ҳ����TextMeshPro���
    public int currentPageIndex = -1; // ��ǰҳ����������ʼ��Ϊ -1 ��ʾû��ҳ��ʾ

    private int totalPages; // ��ҳ��

    private void Start()
    {
        totalPages = pages.Count;

        // ��ʼ������ҳ��Ϊ�洢λ��
        foreach (var page in pages)
        {
            page.localPosition = storagePosition;
        }

        UpdatePageText(); // ��ʼ����ҳ�룬��ʾ0ҳ
    }

    // �����Ķ���������ҳ���ƶ�����ʼλ�ò�����ҳ����ʾ
    public void EndReading()
    {
        for (int i = 0; i < totalPages; i++)
        {
            // ��ÿһҳ�ƶ�������λ�ò�˲�Ƶ��洢λ��
            pages[i].DOLocalMove(recyclePosition, moveDuration).SetEase(easeType).OnComplete(() =>
            {
                pages[i].localPosition = storagePosition;
            });
        }

        currentPageIndex = -1; // ����ҳ��Ϊ 0����ʾû��ҳ��ʾ
        UpdatePageText();
    }

    // ��ʾ��ǰҳ������ҳ���л�
    private void ShowPage(int pageIndex)
    {
        if (pageIndex != currentPageIndex)
        {
            // ���û��ҳ��ʾ����Ҫ��ʾ��һҳ
            if (currentPageIndex == -1 && pageIndex == 0)
            {
                // ֱ����ʾ��һҳ
                pages[pageIndex].DOLocalMove(displayPosition, moveDuration).SetEase(easeType);
            }
            else if (currentPageIndex != -1)
            {
                // �ѵ�ǰҳ�ƶ�������λ�ò�˲�䷵�ش洢λ��
                pages[currentPageIndex].DOLocalMove(recyclePosition, moveDuration).SetEase(easeType).OnComplete(() =>
                {
                    pages[currentPageIndex].localPosition = storagePosition; // ˲�䷵�ش洢λ��
                });

                // �л�����ҳ�沢�ƶ�����ʾλ��
                pages[pageIndex].DOLocalMove(displayPosition, moveDuration).SetEase(easeType);
            }

            currentPageIndex = pageIndex; // ���µ�ǰҳ����
            UpdatePageText(); // ����ҳ����ʾ
        }
    }

    // ����ҳ����ʾ
    private void UpdatePageText()
    {
        if (currentPageIndex == -1)
        {
            pageText.text = $"0/{totalPages}"; // ��ʾ 0 ҳ
        }
        else
        {
            pageText.text = $"{currentPageIndex + 1}/{totalPages}";
        }
    }

    // ��ʾ��һҳ
    public void ShowNextPage()
    {
        if (currentPageIndex < totalPages - 1)
        {
            ShowPage(currentPageIndex + 1); // �л�����һҳ
        }
    }

    // ��ʾ��һҳ���߼�
    public void ShowPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            // ��ǰҳ�����ش洢λ��
            pages[currentPageIndex].DOLocalMove(storagePosition, moveDuration).SetEase(easeType);

            // ��һҳ˲���ƶ�������λ��
            pages[currentPageIndex - 1].localPosition = recyclePosition;

            // Ȼ��ƽ���شӻ���λ���ƶ�����ʾλ��
            pages[currentPageIndex - 1].DOLocalMove(displayPosition, moveDuration).SetEase(easeType);

            currentPageIndex--; // ���µ�ǰҳ����
            UpdatePageText(); // ����ҳ����ʾ
        }
    }

    // Э���ӳ�ִ�� ShowNextPage
    public IEnumerator DelayShowNextPage(float delay)
    {
        yield return new WaitForSeconds(delay); // �ȴ�ָ��ʱ��
        ShowNextPage(); // �ӳٺ���ʾ��һҳ
    }

    // ����Э���ӳ�ִ�з���
    public void StartDelayedShowNextPage(float delay = 1.5f)
    {
        StartCoroutine(DelayShowNextPage(delay));
    }
}
