using UnityEngine;
using UnityEngine.UI;

public class ClothingDesign : MonoBehaviour
{
    [HideInInspector] public Image collar, bottom, sleeves;
    [HideInInspector] public Image overlay, material;

    private void Start()
    {
        collar = transform.GetChild(0).GetComponent<Image>();
        bottom = transform.GetChild(1).GetComponent<Image>();
        sleeves = transform.GetChild(2).GetComponent<Image>();
        overlay = transform.GetChild(3).GetComponent<Image>();
        material = transform.GetChild(4).GetComponent<Image>();
    }
}
