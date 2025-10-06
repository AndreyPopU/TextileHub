using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OverlayPicker : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        ClothingManager.instance.SetOverlay(GetComponent<Image>().sprite);
        transform.GetChild(0).gameObject.SetActive(true);
        ClothingManager.instance.currentOverlay = this;
    }
}
