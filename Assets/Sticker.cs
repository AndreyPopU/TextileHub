using UnityEngine;

public class Sticker : MonoBehaviour
{
    public bool dragging = false;

    private Vector3 offset;

    private void Start()
    {
        offset = Camera.main.transform.position;
    }

    private void Update()
    {
        if (dragging) transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
    }

    private void OnMouseDown()
    {
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
