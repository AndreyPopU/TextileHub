using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StickerPicker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index = 0;

    private Sticker sticker;

    public void OnBeginDrag(PointerEventData data)
    {
        sticker = Instantiate(ClothingManager.instance.stickerPrefabs[index], Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity).GetComponent<Sticker>();
        sticker.dragging = true;
    }

    public void OnDrag(PointerEventData data) // Neccessary for drag to work at all
    {

    }

    public void OnEndDrag(PointerEventData data)
    {
        sticker.dragging = false;
        sticker.CheckCollision();
        sticker = null;
    }
}
