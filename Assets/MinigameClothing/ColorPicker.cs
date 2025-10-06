using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        ClothingManager.instance.SetColor(GetComponent<Image>().color);
        transform.GetChild(0).gameObject.SetActive(true);
        ClothingManager.instance.currentColor = this;
    }
}
