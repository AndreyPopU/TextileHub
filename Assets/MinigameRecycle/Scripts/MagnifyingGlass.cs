using UnityEngine;
using UnityEngine.EventSystems;

public class MagnifyingGlass : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) => canvasGroup.blocksRaycasts = false; // Ignore raycasts while dragging

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Follow the cursor

    public void OnEndDrag(PointerEventData eventData) => canvasGroup.blocksRaycasts = true; // Enable raycasts again
}
