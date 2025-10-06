using UnityEngine;

public class ClothingPiece : MonoBehaviour
{
    public bool mainPiece; // Biggest piece of clothing that all other pieces will be connected to
    public bool dragging = false;

    [HideInInspector] public SpriteRenderer overlay;
    [HideInInspector] public SpriteRenderer material;

    private Vector3 offset;
    private GameObject outline;
    private BoxCollider2D coreCollider;

    private void Start()
    {
        coreCollider = GetComponent<BoxCollider2D>();
        outline = transform.GetChild(0).gameObject;
        overlay = transform.GetChild(1).GetComponent<SpriteRenderer>();
        material = transform.GetChild(2).GetComponent<SpriteRenderer>();

        // Adjust outline scale based on the core collider
        outline.transform.localScale = new Vector3(coreCollider.bounds.extents.x + .1f, coreCollider.bounds.extents.y + .1f, 1);

        // Align mask with sprite
        GetComponent<SpriteMask>().sprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void Update() { if (dragging) transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset; } // Drag

    private void OnMouseEnter() => outline.SetActive(true); // Enable outline

    private void OnMouseExit() => outline.SetActive(false); // If isn't selected, disable outline

    private void OnMouseDown() // Start dragging
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
        ClothingManager.instance.currentPiece = this;
    }

    private void OnMouseUp() { dragging = false; CheckCollision(); } // Stop dragging and check if it connects with the main piece of clothing

    public void CheckCollision() // Check all overlaping colliders that are on the clothing layer, if any collider is hit, attach the sticker to that clothing piece
    {
        if (mainPiece) return;

        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, new Vector2(coreCollider.bounds.extents.x * 2, coreCollider.bounds.extents.y * 2), 0f);

        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i] == coreCollider || !hit[i].GetComponent<ClothingPiece>().mainPiece) continue;
                    
                transform.SetParent(hit[i].transform);
                return;
            }
        }
        transform.SetParent(null);
    }

    // Draw collisions
    private void OnDrawGizmos() { if (coreCollider != null) Gizmos.DrawWireCube(transform.position, new Vector3(coreCollider.bounds.extents.x * 2, coreCollider.bounds.extents.y * 2, 1)); }
}
