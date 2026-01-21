using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DesignDisabler : MonoBehaviour
{
    [SerializeField] GameObject[] interactable_buttons;

    public void Button_Disable()
    {
        foreach (GameObject item in interactable_buttons)
        {
            item.SetActive(false);
        }
    }

    public void Button_Enable()
    {
        foreach (GameObject item in interactable_buttons)
        {
            item.SetActive(true);
        }
    }
}
