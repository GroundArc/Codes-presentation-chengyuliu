using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropContainer : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, ICancelHandler
{
    [HideInInspector]
    public bool inside;

    public RectTransform goodPlaceRef;
    public void OnCancel(BaseEventData eventData)
    {
        inside = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter " + this);
        inside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
