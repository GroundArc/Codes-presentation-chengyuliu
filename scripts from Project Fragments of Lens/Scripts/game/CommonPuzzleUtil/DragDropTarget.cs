using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropTarget : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    Canvas canvas;

    RectTransform rectTrans;

    [SerializeField]
    RectTransform draggingParent;

    public DragDropContainer[] containers;
    [SerializeField]
    DragDropContainer _startDDC;
    Vector3 _startPos;
    public bool freeDrag;
    public enum EndFeedback
    {
        None,
        Disable,
        Destroy,
        FadeImage,
        FadeImageAndUp,
    }
    public EndFeedback endFeedback;

    public enum Showup
    {
        None,
        Fade,
        Scale,
    }
    public Showup showup;

    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        SetToDragDropContrainer(_startDDC);
        switch (showup)
        {
            case Showup.None:
                break;
            case Showup.Fade:
                var img = GetComponent<Image>();
                img.color = new Color(1, 1, 1, 0);
                img.DOKill();
                img.DOFade(1, 0.8f);
                break;
            case Showup.Scale:
                var s = rectTrans.localScale;
                rectTrans.localScale = Vector3.zero;
                rectTrans.DOKill();
                rectTrans.DOScale(s, 0.8f).SetEase(Ease.OutBounce);
                break;
        }
    }

    void SetToDragDropContrainer(DragDropContainer ddc)
    {
        _startDDC = ddc;
        if (ddc != null)
        {
            rectTrans.SetParent(ddc.transform);
            rectTrans.anchoredPosition = ddc.goodPlaceRef.anchoredPosition;
        }

        GetComponent<Image>().raycastTarget = true;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPos = rectTrans.anchoredPosition;
        GetComponent<Image>().raycastTarget = false;
        rectTrans.SetParent(draggingParent.transform);
        //Debug.Log("OnBeginDrag ");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (freeDrag)
        {
            GetComponent<Image>().raycastTarget = true;
            return;
        }

        DragDropContainer endContainer = null;
        foreach (var ddc in containers)
        {
            if (ddc.inside)
            {
                //Debug.Log("OnEndDrag " + ddc);
                endContainer = ddc;
                break;
            }
        }

        if (endContainer == null)
        {
            SetToDragDropContrainer(_startDDC);
        }
        else
        {
            SetToDragDropContrainer(endContainer);
            ApplyEndFeedback();
        }

        foreach (var ddc in containers)
        {
            if (ddc != endContainer)
                ddc.inside = false;
        }
    }

    public void ApplyEndFeedback()
    {
        switch (endFeedback)
        {
            case EndFeedback.None:
                break;
            case EndFeedback.Disable:
                this.enabled = false;
                break;
            case EndFeedback.Destroy:
                Destroy(this.gameObject);
                break;
            case EndFeedback.FadeImage:
                GetComponent<Image>().DOFade(0, 1.2f).OnComplete(
                    () => { Destroy(this.gameObject); });
                break;
            case EndFeedback.FadeImageAndUp:
                var img = GetComponent<Image>();
                img.DOKill();
                var v = rectTrans.anchoredPosition.y + 40;
                rectTrans.DOAnchorPosY(v, 0.8f).SetEase(Ease.InBack).OnComplete(
                    () =>
                    {
                        img.DOFade(0, 0.8f).OnComplete(
                   () => { Destroy(this.gameObject); });
                    });
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown ");
    }
}
