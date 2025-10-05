using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClothingManager : MonoBehaviour
{
    public static ClothingManager instance;

    public bool cursorDebug;
    public ClothingPiece currentPiece;
    public GameObject[] stickerPrefabs;

    [HideInInspector]
    public ColorPicker currentColor;

    private void Awake() => instance = this;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) FinishClothing();

        if (!cursorDebug) return;

        // Raycast from mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            Debug.Log("UI Object under mouse: " + results[0].gameObject.name);
        }
        else
        {
            Debug.Log("No UI object under mouse.");
        }
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
        if (currentPiece != null) currentPiece.GetComponent<SpriteRenderer>().color = color;
    }

    public void OpenMenu(GameObject menu) => menu.SetActive(!menu.activeInHierarchy);
}
