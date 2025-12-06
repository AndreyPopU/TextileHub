using UnityEngine;

public class Sticker : MonoBehaviour
{
    public bool dragging = false;

    private Vector3 offset;
    private BoxCollider2D coreCollider;

    private void Start()
    {
        offset = Camera.main.transform.position - Vector3.up;
        coreCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() { if (dragging) transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset; } // Follow mouse

    private void OnMouseDown() => dragging = true;

    private void OnMouseUp() { dragging = false; CheckCollision(); }

    public void CheckCollision() // Check all overlaping colliders that are on the clothing layer, if any collider is hit, attach the sticker to that clothing piece
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 1.5f, 0f);

        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i] == coreCollider) continue;

                transform.SetParent(hit[i].transform);
                return;
            }
        }
        
        transform.SetParent(null);
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(transform.position, Vector3.one * 1.5f);
}
