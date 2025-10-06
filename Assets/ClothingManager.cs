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
        if (Input.GetKeyDown(KeyCode.F)) FinishClothing(); // Debug for completing an entire clothing

        if (!cursorDebug) return;

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

    public void SetColor(Color color)
    {
        if (currentColor != null) currentColor.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline
        if (currentPiece != null)
        {
            if (primaryColor) currentPiece.material.color = color;
            else currentPiece.overlay.color = color;
        }
    }

    public void SetOverlay(Sprite sprite)
    {
        if (currentOverlay != null) currentOverlay.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline
        if (currentPiece != null) currentPiece.overlay.sprite = sprite;
    }

    public void SetMaterial(Sprite sprite)
    {
        if (currentMaterial != null) currentMaterial.transform.GetChild(0).gameObject.SetActive(false); /// Disable outline
        if (currentPiece != null) currentPiece.material.sprite = sprite;
    }

    public void ChangePrimaryColor() => primaryColor = !primaryColor;

    public void OpenMenu(GameObject menu) => menu.SetActive(!menu.activeInHierarchy);
}
