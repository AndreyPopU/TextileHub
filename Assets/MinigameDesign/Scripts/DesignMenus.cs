using UnityEngine;

public class DesignMenus : MonoBehaviour
{

    [SerializeField] GameObject[] design_menus;
    [SerializeField] GameObject[] sub_menus;

    bool in_menu = false;
    bool fabric_menu = false;
    bool colour1_menu = false;
    bool colour2_menu = false;
    bool pattern_menu = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide();
        Hide_sub();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Hide()
    {
        foreach(GameObject menu in design_menus)
        {
            menu.SetActive(false);
            in_menu = false;
        }
    }

    void Hide_sub()
    {
        foreach (GameObject menu in sub_menus)
        {
            menu.SetActive(false);
        }
    }

    public void MenuFabric()
    {
        if (in_menu == true)
        {
            Hide();
        }
        if (fabric_menu == false)
        {
            fabric_menu = true;
            in_menu = true;
            design_menus[0].SetActive(true);
        }
        else if (fabric_menu == true)
        {
            Hide();
            fabric_menu = false;
        }
    }

    public void MenuColour1()
    {
        if (in_menu == true)
        {
            Hide();
        }
        if (colour1_menu == false)
        {
            colour1_menu = true;
            in_menu = true;
            design_menus[1].SetActive(true);
        }
        else if (colour1_menu == true)
        {
            Hide();
            colour1_menu = false;
        }
    }

    public void MenuColour2()
    {
        if (in_menu == true)
        {
            Hide();
        }
        if (colour2_menu == false)
        {
            colour2_menu = true;
            in_menu = true;
            design_menus[2].SetActive(true);
        }
        else if (colour2_menu == true)
        {
            Hide();
            colour2_menu = false;
        }
    }

    public void MenuPattern()
    {
        if (in_menu == true)
        {
            Hide();
        }
        if (pattern_menu == false)
        {
            pattern_menu = true;
            in_menu = true;
            design_menus[3].SetActive(true);
        }
        else if (pattern_menu == true)
        {
            Hide();
            pattern_menu = false;
        }
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
}
