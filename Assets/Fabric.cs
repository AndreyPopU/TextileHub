using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public Image skin;
    public Sprite [] skinSprites;
    public ShirtResults shirtResults;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        lastPosition = transform.position;
    }

    private void Start () => shirtResults = FindFirstObjectByType<ShirtResults>(); 

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
                if (progress >= 100)
             
                {
                int index = 0;
                   if (shirtResults.isAzo)
                   {
                        index = 5;
                   }
                    else if (shirtResults.isCO2) 
                    {
                        index = 10;
                    }
                   skin.sprite = skinSprites [shirtResults.results[4] + index];
                }
            }
        }
    }
}