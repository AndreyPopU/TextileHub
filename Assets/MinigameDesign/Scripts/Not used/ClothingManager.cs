using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClothingManager : MonoBehaviour
{
    public static ClothingManager instance;

    public bool cursorDebug;
    public bool primaryColor = true; // If the player is coloring the primary or secondary color

    public ClothingDesign clothing;

    [Header("Pieces")]
    public ClothingPiece currentPiece;
    public GameObject[] stickerPrefabs;

    // Store current clothing options
    [HideInInspector] public GameObject currentColorPrimary, currentcolorSecondary;

    private void Awake() => instance = this;

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
        // Disable stickers that are outside of clothing pieces
        Sticker[] stickers = FindObjectsByType<Sticker>(FindObjectsSortMode.None);

        for (int i = 0; i < stickers.Length; i++)
            stickers[i].GetComponentInChildren<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }


    #region Designing Clothes

    public void SetPrimaryColor(Image image)
    {
        if (currentColorPrimary != null) currentColorPrimary.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline of previously selected collor
        clothing.collar.color = image.color;
        clothing.bottom.color = image.color;
        clothing.sleeves.color = image.color;
        currentColorPrimary = image.gameObject;
        currentColorPrimary.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetSecondaryColor(Image image)
    {
        if (currentcolorSecondary != null) currentcolorSecondary.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline of previously selected collor
        clothing.overlay.color = image.color;
        currentcolorSecondary = image.gameObject;
        currentcolorSecondary.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ChangePrimaryColor() => primaryColor = !primaryColor; // Cycle between primary and secondary color

    public void SetOverlay(Image image) => clothing.overlay.sprite = image.sprite;

    public void SetMaterial(Image image) => clothing.material.sprite = image.sprite;

    public void SetCollar(Sprite sprite) => clothing.collar.sprite = sprite;

    public void SetBottom(Sprite sprite) => clothing.bottom.sprite = sprite;

    public void SetSleeves(Sprite sprite) => clothing.sleeves.sprite = sprite;

    #endregion
}
