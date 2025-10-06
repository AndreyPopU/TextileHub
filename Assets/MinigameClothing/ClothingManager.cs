using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClothingManager : MonoBehaviour
{
    public static ClothingManager instance;

    public bool cursorDebug;
    public bool primaryColor = true; // If the player is coloring the primary or secondary color
    public ClothingPiece currentPiece;
    public GameObject[] stickerPrefabs;

    // Store current clothing options
    [HideInInspector] public ColorPicker currentColor;
    [HideInInspector] public OverlayPicker currentOverlay;
    [HideInInspector] public MaterialPicker currentMaterial;

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

        // Get all clothing pieces
        ClothingPiece[] clothingPieces = FindObjectsByType<ClothingPiece>(FindObjectsSortMode.None);

        for (int i = 0; i < clothingPieces.Length; i++)
        {
            // Center main piece
            if (clothingPieces[i].mainPiece)
            {
                clothingPieces[i].transform.SetParent(transform);
                clothingPieces[i].transform.localPosition = Vector2.zero;
            }
            else
            {
                // If clothing piece has no parent (isn't attached to the main piece) - Destroy it
                if (clothingPieces[i].transform.parent == null)
                {
                    Destroy(clothingPieces[i].gameObject);
                    continue;
                }
            }

            // Remove all clothing piece logic
            Destroy(clothingPieces[i]);
        }
    }

    public void SetColor(Color color)
    {
        if (currentColor != null) currentColor.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline of previously selected collor
        if (currentPiece != null) 
        {
            // Set either primary or secondary color of current piece
            if (primaryColor) currentPiece.material.color = color;
            else currentPiece.overlay.color = color;
        }
    }

    public void SetOverlay(Sprite sprite)
    {
        if (currentOverlay != null) currentOverlay.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline of previously selected Overlay
        if (currentPiece != null) currentPiece.overlay.sprite = sprite;
    }

    public void SetMaterial(Sprite sprite)
    {
        if (currentMaterial != null) currentMaterial.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline of previously selected Material
        if (currentPiece != null) currentPiece.material.sprite = sprite;
    }

    public void ChangePrimaryColor() => primaryColor = !primaryColor; // Cycle between primary and secondary color

    public void OpenMenu(GameObject menu) => menu.SetActive(!menu.activeInHierarchy); // Activate a certain UI menu
}
