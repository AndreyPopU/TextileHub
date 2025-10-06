using UnityEngine;
using UnityEngine.EventSystems;

public class StickerPicker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index = 0;

    private Sticker sticker;

    public void OnBeginDrag(PointerEventData data) // Spawn an actual sticker and start dragging it
    {
        sticker = Instantiate(ClothingManager.instance.stickerPrefabs[index], Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity).GetComponent<Sticker>();
        sticker.dragging = true;
    }

    public void OnDrag(PointerEventData data) // Neccessary for drag to work at all
    {

    }

    public void OnEndDrag(PointerEventData data) // Release spawned sticker
    {
        sticker.dragging = false;
        sticker.CheckCollision(); // Check if it collides with a clothing piece - if it does, attach it
        sticker = null;
    }
}
