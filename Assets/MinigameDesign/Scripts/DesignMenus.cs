using UnityEngine;

public class DesignMenus : MonoBehaviour
{
    #region --Fields and properties--
    [Header("Object Arrays")]
    [SerializeField] GameObject[] design_menus;
    #endregion

    void Start()
    {
        Hide();
    }

    public void Show(GameObject menu)
    {
        if (menu.activeSelf)
            {
            Hide();
            }
        else
            {
            Hide();
            menu.SetActive(true);
            }            
    }

    public void Hide()
    {
        foreach(GameObject menu in design_menus)
        {
            menu.SetActive(false);
        }
    }
}
