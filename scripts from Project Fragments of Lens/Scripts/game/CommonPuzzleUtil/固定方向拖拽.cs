using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 固定方向拖拽 : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    Canvas canvas;

    RectTransform _rectTrans;

    public RectTransform end1;
    public RectTransform end2;
    private void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
        GetComponent<Image>().raycastTarget = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var idealPos = _rectTrans.anchoredPosition + eventData.delta / canvas.scaleFactor;
        idealPos = NearestPointOnLine(idealPos, end1.anchoredPosition, end2.anchoredPosition);
        _rectTrans.anchoredPosition = idealPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = true;
    }

    public static Vector2 NearestPointOnLine(Vector2 A, Vector2 B, Vector2 C)
    {
        // Calculate the direction vector of the line
        Vector2 BC = C - B;
        // Calculate the vector from point A to point B
        Vector2 BA = A - B;
        // Calculate the projection of BA onto BC
        float t = Vector2.Dot(BA, BC) / Vector2.Dot(BC, BC);
        // Ensure t is within the line segment BC
        t = Mathf.Clamp01(t);
        // Calculate the nearest point on the line
        Vector2 nearestPoint = B + t * BC;
        return nearestPoint;
    }
}