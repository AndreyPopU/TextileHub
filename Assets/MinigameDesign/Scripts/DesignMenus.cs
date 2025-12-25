using UnityEngine;

public class DesignMenus : MonoBehaviour
{
    #region --Fields and properties--
    [Header("Object Arrays")]
    [SerializeField] GameObject[] design_menus;
    [SerializeField] GameObject[] sub_menus;
    #endregion

    void Start()
    {
        Hide();
        Hide_sub();
    }

    public void ShowChild(GameObject child)
    {
        foreach (GameObject menu in sub_menus)
        {
            if (menu.activeSelf)
                Hide_sub();
            continue;
        }
        child.SetActive(!child.activeSelf);
    }

    void Hide()
    {
        foreach(GameObject menu in design_menus)
        {
            menu.SetActive(false);
        }
    }

    void Hide_sub()
    {
        foreach (GameObject menu in sub_menus)
        {
            menu.SetActive(false);
        }
    }
}
