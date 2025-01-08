using UnityEngine;
using DG.Tweening;

public class ClickMove : MonoBehaviour
{
    public RectTransform targetRect;
    public Vector2 startPos;
    public Vector2 endPos;
    public float duration;
    public bool canFlip;
    public bool canRepeat;
    public Ease ease;

    private bool _moving;
    private int movedHalfTurns;

    void Start()
    {
        movedHalfTurns = 0;
        StartMove(true, false);
    }

    public void StartMove(bool instant, bool endOrStart)
    {
        if (instant)
        {
            _moving = false;
            targetRect.DOKill();
            targetRect.anchoredPosition = endOrStart ? endPos : startPos;
            return;
        }

        _moving = true;
        targetRect.DOKill();
        targetRect.DOAnchorPos(endOrStart ? endPos : startPos, duration)
            .SetEase(ease)
            .OnComplete(
            () => { _moving = false; }
            );
    }

    public void OnClick()
    {
        if (_moving) return;

        if (!canRepeat && movedHalfTurns >= 2)
            return;

        if (!canFlip && movedHalfTurns % 2 == 1)
            return;

        StartMove(false, (movedHalfTurns % 2 == 0));
        movedHalfTurns++;
    }
}