using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class DesignChanger : MonoBehaviour
{
    #region --Fields and properties--
    public static DesignChanger instance;

    [Header("Image Arrays")]
    [SerializeField] Sprite[] colars;
    [SerializeField] Sprite[] sleeves;
    [SerializeField] Sprite[] hems;
    [SerializeField] Sprite[] l_colars;
    [SerializeField] Sprite[] l_sleeves;
    [SerializeField] Sprite[] l_hems;

    [Header("Object Arrays")]
    [SerializeField] GameObject[] patterns;

    [Header("Saving Data")]
    public int[] selectedProperties; // Collar, Sleeves, Bottom, Overlay, Material
    public string primaryHex, secondaryHex;
    public bool finishedDesign;
    public int colar_index;
    public int sleeves_index;
    public int hem_index;

    [Header("Colour Data")]
    public string current_colour_1;
    public string current_colour_2;
    public string color1Hex, color2Hex;

    [Header("Object References")]
    [SerializeField] GameObject obj_colar;
    [SerializeField] GameObject obj_sleeves;
    [SerializeField] GameObject obj_hem;
    [SerializeField] GameObject l_obj_colar;
    [SerializeField] GameObject l_obj_sleeves;
    [SerializeField] GameObject l_obj_hem;
    [SerializeField] MoneyManager moneyManager;

    [Header("Private")]
    Image color_image;
    Color image_color;

    float _cost1;
    float _cost2;

    int cost_dye_azo = 10;
    int cost_dye_reactive = 20;
    int cost_dye_co2 = 30;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        selectedProperties = new int[5];
    }

    public void FinishClothing()
    {
        if (MoneyManager.instance != null && MoneyManager.instance.budget < 0) return;

        if (finishedDesign) return;

        // Collar, Sleeves, Bottom, Overlay, Material
        selectedProperties[0] = colar_index;
        selectedProperties[1] = sleeves_index;
        selectedProperties[2] = hem_index;
        selectedProperties[3] = GetComponent<DesignFabric>().index;
        selectedProperties[4] = GetComponent<DesignPattern>().index;

        // Transfer colors
        primaryHex = color1Hex;
        secondaryHex = color2Hex;

        for (int i = 0; i < selectedProperties.Length; i++) Debug.Log($"{selectedProperties[i]}");

        print(primaryHex);
        print(secondaryHex);

        // Send properties after they've been set
        if (WebSocketClient.instance != null)
        {
            var designMessage = new FinalDesignMessage
            {
                type = "finaldesign",
                playerId = FindFirstObjectByType<WebSocketClient>().localPlayerId,
                designResults = selectedProperties,
                primaryHex = primaryHex,
                secondaryHex = secondaryHex,
            };

            string json = JsonUtility.ToJson(designMessage);
            WebSocketClient.instance.SendMessageToServer(json);
        }

        finishedDesign = true;
    }


    public void SetCollar(int index)
    {
        Debug.Log($"Setting COLLAR to index [{index}] which is {colars[index]} and {l_colars[index]}");
        obj_colar.GetComponent<Image>().sprite = colars[index];
        l_obj_colar.GetComponent<Image>().sprite = l_colars[index];
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

    #region --Clothing Pieces--
    //------Collar------

    public void OnButtonCollarLeft()
    {
        colar_index = colar_index - 1;
        Change_Colar();
    }

    public void OnButtonCollarRight()
    {
        colar_index = colar_index + 1;
        Change_Colar();
    }

    void Change_Colar()
    {
        if (colar_index > 4)
            colar_index = 0;

        if (colar_index < 0)
            colar_index = 4;

        obj_colar.GetComponent<Image>().sprite = colars[colar_index];
        l_obj_colar.GetComponent<Image>().sprite = l_colars[colar_index];
    }

    //------Sleeves------

    public void OnButtonSleevesLeft()
    {
        sleeves_index = sleeves_index - 1;
        Change_Sleeves();
    }

    public void OnButtonSleevesRight()
    {
        sleeves_index = sleeves_index + 1;
        Change_Sleeves();
    }

    void Change_Sleeves()
    {
        if (sleeves_index > 4)
            sleeves_index = 0;

        if (sleeves_index < 0)
            sleeves_index = 4;

        obj_sleeves.GetComponent<Image>().sprite = sleeves[sleeves_index];
        l_obj_sleeves.GetComponent<Image>().sprite = l_sleeves[sleeves_index];
    }

    //------Hem------

    public void OnButtonHemLeft()
    {
        hem_index = hem_index - 1;
        Change_Hem();
    }

    public void OnButtonHemRight()
    {
        hem_index = hem_index + 1;
        Change_Hem();
    }

    void Change_Hem()
    {
        if (hem_index > 3)
            hem_index = 0;

        if (hem_index < 0)
            hem_index = 3;

        obj_hem.GetComponent<Image>().sprite = hems[hem_index];
        l_obj_hem.GetComponent<Image>().sprite = l_hems[hem_index];
    }
    #endregion

    #region --Set Colors--
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
        obj_colar.GetComponent<Image>().color = color;
        l_obj_colar.GetComponent<Image>().color = color;
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
    #endregion

    #region --Colors--
    public void Colour1(string colour)
    {
        switch (colour)
        {
            case "red": image_color = new Color32(242, 44, 44, 255); Money1(cost_dye_azo); break;
            case "red1": image_color = new Color32(213, 13, 70, 255); Money1(cost_dye_reactive); break;
            case "red2": image_color = new Color32(209, 77, 16, 255); Money1(cost_dye_co2); break;
            case "yellow": image_color = new Color32(232, 200, 39, 255); Money1(cost_dye_azo); break;
            case "yellow1": image_color = new Color32(232, 157, 39, 255); Money1(cost_dye_reactive); break;
            case "yellow2": image_color = image_color = new Color32(246, 242, 114, 255); Money1(cost_dye_co2); break;
            case "green": image_color = new Color32(27, 168, 41, 255); Money1(cost_dye_azo); break;
            case "green1": image_color = new Color32(27, 90, 41, 255); Money1(cost_dye_reactive); break;
            case "green2": image_color = new Color32(27, 160, 130, 255); Money1(cost_dye_co2); break;
            case "blue": image_color = new Color32(0, 131, 225, 255); Money1(cost_dye_azo); break;
            case "blue1": image_color = new Color32(0, 50, 225, 255); Money1(cost_dye_reactive); break;
            case "blue2": image_color = new Color32(80, 110, 140, 255); Money1(cost_dye_co2); break;
            case "purple": image_color = new Color32(170, 0, 225, 255); Money1(cost_dye_azo); break;
            case "purple1": image_color = new Color32(114, 0, 225, 255); Money1(cost_dye_reactive); break;
            case "purple2": image_color = new Color32(80, 0, 156, 255); Money1(cost_dye_co2); break;
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
        switch (colour)
        {
            case "red": image_color = new Color32(242, 44, 44, 255); Money2(cost_dye_azo); break;
            case "red1": image_color = new Color32(213, 13, 70, 255); Money2(cost_dye_reactive); break;
            case "red2": image_color = new Color32(209, 77, 16, 255); Money2(cost_dye_co2); break;
            case "yellow": image_color = new Color32(232, 200, 39, 255); Money2(cost_dye_azo); break;
            case "yellow1": image_color = new Color32(232, 157, 39, 255); Money2(cost_dye_reactive); break;
            case "yellow2": image_color = image_color = new Color32(246, 242, 114, 255); Money2(cost_dye_co2); break;
            case "green": image_color = new Color32(27, 168, 41, 255); Money2(cost_dye_azo); break;
            case "green1": image_color = new Color32(27, 90, 41, 255); Money2(cost_dye_reactive); break;
            case "green2": image_color = new Color32(27, 160, 130, 255); Money2(cost_dye_co2); break;
            case "blue": image_color = new Color32(0, 131, 225, 255); Money2(cost_dye_azo); break;
            case "blue1": image_color = new Color32(0, 50, 225, 255); Money2(cost_dye_reactive); break;
            case "blue2": image_color = new Color32(80, 110, 140, 255); Money2(cost_dye_co2); break;
            case "purple": image_color = new Color32(170, 0, 225, 255); Money2(cost_dye_azo); break;
            case "purple1": image_color = new Color32(114, 0, 225, 255); Money2(cost_dye_reactive); break;
            case "purple2": image_color = new Color32(80, 0, 156, 255); Money2(cost_dye_co2); break;
        }

        current_colour_2 = colour;
        color2Hex = image_color.ToHexString();

        foreach (GameObject item in patterns)
        {
            color_image = item.GetComponent<Image>();
            color_image.color = image_color;
        }
    }
    #endregion

    #region --Color cost--
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
    #endregion

}
