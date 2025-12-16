using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClothingManager : MonoBehaviour
{
    public static ClothingManager instance;

    public bool finishedDesign;
    public bool cursorDebug;
    public bool primaryColor = true; // If the player is coloring the primary or secondary color

    public ClothingDesign clothing;

    public Sprite[] properties; // 0-5: collar; 5-10: sleeves; 10-15: bottom; 15-20: overlay; 20-25: material; 25: color; 26: secondary color;
    public int[] selectedProperties;
    public string primaryHex, secondaryHex;

    // Store current clothing options
    [HideInInspector] public GameObject currentColorPrimary, currentcolorSecondary;

    private void Awake() => instance = this;

    private void Start()
    {
        selectedProperties = new int[5];
    }

    private void Update()
    {
        if (!cursorDebug) return; // Debug which object is being hovered by the cursor

        // Raycast from mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0) Debug.Log("UI Object under mouse: " + results[0].gameObject.name);
        else Debug.Log("No UI object under mouse.");
    }

    public void FinishClothing()
    {
        if (finishedDesign) return;

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

    #region Designing Clothes

    public void SetProperty(int property) // 0-5: collar; 5-10: sleeves; 10-15: bottom; 15-20: overlay; 20-25: material; 25: color; 26: secondary color;
    {
        if (property < 5)
        {
            clothing.collar.sprite = properties[property];
            selectedProperties[0] = property;
        }
        else if (property >= 5 && property < 10)
        {
            clothing.sleeves.sprite = properties[property];
            selectedProperties[1] = property;
        }
        else if (property >= 10 && property < 15)
        {
            clothing.bottom.sprite = properties[property];
            selectedProperties[2] = property;
        }
        else if (property >= 15 && property < 20)
        {
            clothing.overlay.sprite = properties[property];
            selectedProperties[3] = property;
        }
        else if (property >= 20 && property < 25)
        {
            clothing.material.sprite = properties[property];
            selectedProperties[4] = property;
        }
    }

    public void ResultSetPrimaryColor(string newHex)
    {
        newHex = "#" + newHex;
        print(newHex);
        UnityEngine.ColorUtility.TryParseHtmlString(newHex, out Color resultColor);
        print("result color is " + resultColor);
        clothing.collar.color = resultColor;
        clothing.bottom.color = resultColor;
        clothing.sleeves.color = resultColor;
    }

    public void ResultSetSecondaryColor(string newHex)
    {
        newHex = "#" + newHex;
        UnityEngine.ColorUtility.TryParseHtmlString(newHex, out Color resultColor);
        clothing.overlay.color = resultColor;
    }

    public void SetPrimaryColor(Image image)
    {
        if (currentColorPrimary != null) currentColorPrimary.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline of previously selected collor
        clothing.collar.color = image.color;
        clothing.bottom.color = image.color;
        clothing.sleeves.color = image.color;
        currentColorPrimary = image.gameObject;
        currentColorPrimary.transform.GetChild(0).gameObject.SetActive(true);
        primaryHex = image.color.ToHexString();
    }

    public void SetSecondaryColor(Image image)
    {
        if (currentcolorSecondary != null) currentcolorSecondary.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline of previously selected collor
        clothing.overlay.color = image.color;
        currentcolorSecondary = image.gameObject;
        currentcolorSecondary.transform.GetChild(0).gameObject.SetActive(true);
        secondaryHex = image.color.ToHexString();
    }

    public void ChangePrimaryColor() => primaryColor = !primaryColor; // Cycle between primary and secondary color

    #endregion
}
