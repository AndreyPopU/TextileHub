using UnityEngine;

public class ClothingPiece : MonoBehaviour
{
    public bool dragging = false;

    private Vector3 offset;
    private GameObject outline;

    private void Start()
    {
        outline = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (dragging) transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
    }

    private void OnMouseEnter()
    {
        outline.SetActive(true);
    }

    private void OnMouseExit()
    {
        outline.SetActive(false);
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
        ClothingManager.instance.currentPiece = this;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
