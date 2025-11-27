using UnityEngine;
using UnityEngine.EventSystems;

public class Fabric : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum Outcome { Stretchy, Itchy, Sweaty }

    public Outcome outcome;

    public float progress;

    public RectTransform fabricTransform, skinTransform;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 lastPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        lastPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData) => canvasGroup.blocksRaycasts = false; // Ignore raycasts while dragging

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Follow the cursor

    public void OnEndDrag(PointerEventData eventData) => canvasGroup.blocksRaycasts = true; // Enable raycasts again

    void FixedUpdate()
    {
        if (fabricTransform.Overlaps(skinTransform) && progress < 100) // Check if it's moving as well
        {
            if (transform.position != lastPosition)
            {
                lastPosition = transform.position;
                progress += .5f;
                if (progress >= 100) AsyncLoad.instance.LoadNextScene();
            }
        }
    }

}