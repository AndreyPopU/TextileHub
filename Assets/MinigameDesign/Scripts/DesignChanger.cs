using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class DesignChanger : MonoBehaviour
{
    public static DesignChanger instance;

    [SerializeField] Sprite[] colars;
    [SerializeField] Sprite[] sleeves;
    [SerializeField] Sprite[] hems;
    [SerializeField] Sprite[] l_colars;
    [SerializeField] Sprite[] l_sleeves;
    [SerializeField] Sprite[] l_hems;

    [SerializeField] GameObject[] patterns;

    [SerializeField] GameObject obj_colar;
    [SerializeField] GameObject obj_sleeves;
    [SerializeField] GameObject obj_hem;
    [SerializeField] GameObject l_obj_colar;
    [SerializeField] GameObject l_obj_sleeves;
    [SerializeField] GameObject l_obj_hem;

    [SerializeField] MoneyManager moneyManager;

    public int colar_index;
    public int sleeves_index;
    public int hem_index;

    public string current_colour_1;
    public string current_colour_2;

    public string color1Hex, color2Hex;

    Image color_image;
    Color image_color;

    float _cost1;
    float _cost2;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Colour1("blue");
        Colour2("yellow");
    }

    public void SetCollar(int index)
    {
        obj_sleeves.GetComponent<Image>().sprite = sleeves[index];
        l_obj_sleeves.GetComponent<Image>().sprite = l_sleeves[index];
    }

    public void SetSleeves(int index)
    {
        obj_sleeves.GetComponent<Image>().sprite = sleeves[index];
        l_obj_sleeves.GetComponent<Image>().sprite = l_sleeves[index];
    }

    public void SetHem(int index)
    {
        obj_hem.GetComponent<Image>().sprite = hems[index];
        l_obj_hem.GetComponent<Image>().sprite = l_hems[index];
    }

    public void SetOverlay()
    {

    }

    public void SetMaterial()
    {

    }

    public void ResultSetPrimaryColor(string newHex)
    {
        newHex = "#" + newHex;
        print(newHex);
        UnityEngine.ColorUtility.TryParseHtmlString(newHex, out Color resultColor);
        print("result color is " + resultColor);

        SetCollarColor(resultColor);
        SetSleevesColor(resultColor);
        SetHemColor(resultColor);
    }

    public void ResultSetSecondaryColor(string newHex)
    {
        newHex = "#" + newHex;
        UnityEngine.ColorUtility.TryParseHtmlString(newHex, out Color resultColor);
        instance.SetOverlayColor(resultColor);
    }

    public void SetCollarColor(Color color)
    {
        obj_sleeves.GetComponent<Image>().color = color;
        l_obj_sleeves.GetComponent<Image>().color = color;
    }

    public void SetSleevesColor(Color color)
    {
        obj_sleeves.GetComponent<Image>().color = color;
        l_obj_sleeves.GetComponent<Image>().color = color;
    }

    public void SetHemColor(Color color)
    {
        obj_hem.GetComponent<Image>().color = color;
        l_obj_hem.GetComponent<Image>().color = color;
    }

    public void SetOverlayColor(Color color)
    {
        foreach (GameObject item in patterns)
        {
            color_image = item.GetComponent<Image>();
            color_image.color = color;
        }
    }

    void Money1(float _cost)
    {
        _cost1 = _cost;
        Money();
    }

    void Money2(float _cost)
    {
        _cost2 = _cost;
        Money();
    }

    void Money()
    {
        if (moneyManager == null) return;

        moneyManager.Cost_color(_cost1,_cost2);
    }

    public void Colour1(string colour)
    {
        if (colour == "red")
        {
            image_color = new Color32(242, 44, 44, 255);
            Money1(10);
        }
        else if (colour == "red1")
        { 
            image_color = new Color32(213, 13, 70, 255);
            Money1(20);
        }
        else if (colour == "red2")
        {
            image_color = new Color32(209, 77, 16, 255);
            Money1(30);
        }
        else if (colour == "yellow")
        { 
            image_color = new Color32(232, 200, 39, 255);
            Money1(10);
        }
        else if (colour == "yellow1")
        { 
            image_color = new Color32(232, 157, 39, 255);
            Money1(20);
        }
        else if (colour == "yellow2")
        { 
            image_color = new Color32(246, 242, 114, 255);
            Money1(30);
        }
        else if (colour == "green")
        { 
            image_color = new Color32(27, 168, 41, 255);
            Money1(10);
        }
        else if (colour == "green1")
        { 
            image_color = new Color32(27, 90, 41, 255);
            Money1(20);
        }
        else if (colour == "green2")
        { 
            image_color = new Color32(27, 160, 130, 255);
            Money1(30);
        }
        else if (colour == "blue")
        { 
            image_color = new Color32(0, 131, 225, 255);
            Money1(10);
        }
        else if (colour == "blue1")
        { 
            image_color = new Color32(0, 50, 225, 255);
            Money1(20);
        }
        else if (colour == "blue2")
        { 
            image_color = new Color32(80, 110, 140, 255);
            Money1(30);
        }
        else if (colour == "purple")
        { 
            image_color = new Color32(170, 0, 225, 255);
            Money1(10);
        }
        else if (colour == "purple1")
        { 
            image_color = new Color32(114, 0, 225, 255);
            Money1(20);
        }
        else if (colour == "purple2")
        { 
            image_color = new Color32(80, 0, 156, 255);
            Money1(30);
        }

        current_colour_1 = colour;
        color1Hex = image_color.ToHexString();

        color_image = obj_colar.GetComponent<Image>();
        color_image.color = image_color;
        color_image = obj_sleeves.GetComponent<Image>();
        color_image.color = image_color;
        color_image = obj_hem.GetComponent<Image>();
        color_image.color = image_color;

    }

    public void Colour2(string colour)
    {
        if (colour == "red")
        {
            image_color = new Color32(242, 44, 44, 255);
            Money2(10);
        }
        else if (colour == "red1")
        {
            image_color = new Color32(213, 13, 70, 255);
            Money2(20);
        }
        else if (colour == "red2")
        {
            image_color = new Color32(209, 77, 16, 255);
            Money2(30);
        }
        else if (colour == "yellow")
        {
            image_color = new Color32(232, 200, 39, 255);
            Money2(10);
        }
        else if (colour == "yellow1")
        {
            image_color = new Color32(232, 157, 39, 255);
            Money2(20);
        }
        else if (colour == "yellow2")
        {
            image_color = new Color32(246, 242, 114, 255);
            Money2(30);
        }
        else if (colour == "green")
        {
            image_color = new Color32(27, 168, 41, 255);
            Money2(10);
        }
        else if (colour == "green1")
        {
            image_color = new Color32(27, 90, 41, 255);
            Money2(20);
        }
        else if (colour == "green2")
        {
            image_color = new Color32(27, 160, 130, 255);
            Money2(30);
        }
        else if (colour == "blue")
        {
            image_color = new Color32(0, 131, 225, 255);
            Money2(10);
        }
        else if (colour == "blue1")
        {
            image_color = new Color32(0, 50, 225, 255);
            Money2(20);
        }
        else if (colour == "blue2")
        {
            image_color = new Color32(80, 110, 140, 255);
            Money2(30);
        }
        else if (colour == "purple")
        {
            image_color = new Color32(170, 0, 225, 255);
            Money2(10);
        }
        else if (colour == "purple1")
        {
            image_color = new Color32(114, 0, 225, 255);
            Money2(20);
        }
        else if (colour == "purple2")
        {
            image_color = new Color32(80, 0, 156, 255);
            Money2(30);
        }

        current_colour_2 = colour;
        color2Hex = image_color.ToHexString();

        foreach (GameObject item in patterns)
        {
            color_image = item.GetComponent<Image>();
            color_image.color = image_color;
        }
    }

    //------Collar------

    public void OnButtonCollarLeft()
    {
        colar_index = colar_index - 1;
        Change_Colar();
        obj_colar.GetComponent<Image>().sprite = colars[colar_index];
        l_obj_colar.GetComponent<Image>().sprite = l_colars[colar_index];
    }

    public void OnButtonCollarRight()
    {
        colar_index = colar_index + 1;
        Change_Colar();
        obj_colar.GetComponent<Image>().sprite = colars[colar_index];
        l_obj_colar.GetComponent<Image>().sprite = l_colars[colar_index];
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
    }

    public void OnButtonSleevesRight()
    {
        sleeves_index = sleeves_index + 1;
        Change_Sleeves();
        obj_sleeves.GetComponent<Image>().sprite = sleeves[sleeves_index];
        l_obj_sleeves.GetComponent<Image>().sprite = l_sleeves[sleeves_index];
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
    }

    public void OnButtonHemRight()
    {
        hem_index = hem_index + 1;
        Change_Hem();
        obj_hem.GetComponent<Image>().sprite = hems[hem_index];
        l_obj_hem.GetComponent<Image>().sprite = l_hems[hem_index];
    }

    void Change_Hem()
    {
        if (hem_index > 3)
            hem_index = 0;

        if (hem_index < 0)
            hem_index = 3;
    }
}
