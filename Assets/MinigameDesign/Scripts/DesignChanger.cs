using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class DesignChanger : MonoBehaviour
{

    [SerializeField] Sprite[] colars;
    [SerializeField] Sprite[] sleeves;
    [SerializeField] Sprite[] hems;
    [SerializeField] Sprite[] l_colars;
    [SerializeField] Sprite[] l_sleeves;
    [SerializeField] Sprite[] l_hems;

    [SerializeField] GameObject obj_colar;
    [SerializeField] GameObject obj_sleeves;
    [SerializeField] GameObject obj_hem;
    [SerializeField] GameObject l_obj_colar;
    [SerializeField] GameObject l_obj_sleeves;
    [SerializeField] GameObject l_obj_hem;

    int colar_index;
    int sleeves_index;
    int hem_index;

    Image color_image;
    Color image_color;

    public void Colour1(string colour)
    {
        if (colour == "red")
            image_color = new Color32(242,44,44,255);
        else if (colour == "red1")
            image_color = new Color32(213, 13, 70, 255);
        else if (colour == "red2")
            image_color = new Color32(209, 77, 16, 255);
        else if (colour == "red3")
            image_color = new Color32(228, 24, 160, 255);
        else if (colour == "red4")
            image_color = new Color32(193, 13, 13, 255);

        Debug.Log(image_color);
        color_image = obj_colar.GetComponent<Image>();
        color_image.color = image_color;
        color_image = obj_sleeves.GetComponent<Image>();
        color_image.color = image_color;
        color_image = obj_hem.GetComponent<Image>();
        color_image.color = image_color;

    }

    private void Start()
    {
        
    }

    //------Collar------

    public void OnButtonCollarLeft()
    {
        colar_index = colar_index - 1;
        Change_Colar();
        obj_colar.GetComponent<Image>().sprite = colars[colar_index];
        l_obj_colar.GetComponent<Image>().sprite = l_colars[colar_index];
        Debug.Log(colar_index);
    }

    public void OnButtonCollarRight()
    {
        colar_index = colar_index + 1;
        Change_Colar();
        obj_colar.GetComponent<Image>().sprite = colars[colar_index];
        l_obj_colar.GetComponent<Image>().sprite = l_colars[colar_index];
        Debug.Log(colar_index);
    }

    void Change_Colar()
    {
        if (colar_index > 4)
            colar_index = 0;
        
        if (colar_index < 0)
            colar_index = 4;
    }

    //------Sleeves------

    public void OnButtonSleevesLeft()
    {
        sleeves_index = sleeves_index - 1;
        Change_Sleeves();
        obj_sleeves.GetComponent<Image>().sprite = sleeves[sleeves_index];
        l_obj_sleeves.GetComponent<Image>().sprite = l_sleeves[sleeves_index];
        Debug.Log(sleeves_index);
    }

    public void OnButtonSleevesRight()
    {
        sleeves_index = sleeves_index + 1;
        Change_Sleeves();
        obj_sleeves.GetComponent<Image>().sprite = sleeves[sleeves_index];
        l_obj_sleeves.GetComponent<Image>().sprite = l_sleeves[sleeves_index];
        Debug.Log(sleeves_index);
    }

    void Change_Sleeves()
    {
        if (sleeves_index > 4)
            sleeves_index = 0;

        if (sleeves_index < 0)
            sleeves_index = 4;
    }

    //------Hem------

    public void OnButtonHemLeft()
    {
        hem_index = hem_index - 1;
        Change_Hem();
        obj_hem.GetComponent<Image>().sprite = hems[hem_index];
        l_obj_hem.GetComponent<Image>().sprite = l_hems[hem_index];
        Debug.Log(hem_index);
    }

    public void OnButtonHemRight()
    {
        hem_index = hem_index + 1;
        Change_Hem();
        obj_hem.GetComponent<Image>().sprite = hems[hem_index];
        l_obj_hem.GetComponent<Image>().sprite = l_hems[hem_index];
        Debug.Log(hem_index);
    }

    void Change_Hem()
    {
        if (hem_index > 3)
            hem_index = 0;

        if (hem_index < 0)
            hem_index = 3;
    }
}
