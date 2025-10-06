using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaterialPicker : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        ClothingManager.instance.SetMaterial(GetComponent<Image>().sprite);
        transform.GetChild(0).gameObject.SetActive(true);
        ClothingManager.instance.currentMaterial = this;
    }
}