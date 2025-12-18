using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class RecyclingManager : MonoBehaviour
{
    public static RecyclingManager instance;

    public bool finishedGame;
    public GameObject resultsPanel;
    public CanvasGroup magnifyingGlass;
    public bool glass;
    public int imperfectionsFound;

    private Camera mainCamera;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (imperfectionsFound == 5)
        {
            if (!resultsPanel.activeInHierarchy) resultsPanel.SetActive(true);

            return;
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //    SwitchMode();

        //if (glass) return;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Convert mouse position to world coordinates
        //    Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 mouseWorld2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        //    // Raycast at the mouse position
        //    RaycastHit2D hit = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

        //    if (hit.collider != null) // If raycast hits something
        //    {
        //        if (hit.collider.transform.TryGetComponent(out MaterialFabric fabric)) // Something that is fabric
        //        {
        //            if (fabric.visible) // If fabric is under magnifying glass
        //            {

        //            }
        //        }
        //    }
        //}
    }

    public void SendGuess(int response)
    {
        if (finishedGame) return;

        if (WebSocketClient.instance != null)
        {
            var magnifyingMessage = new MagnifyingMessage
            {
                type = "magnify",
                response = response,
            };

            string json = JsonUtility.ToJson(magnifyingMessage);
            WebSocketClient.instance.SendMessageToServer(json);
        }

        finishedGame = true;
    }

    //public void SwitchMode()
    //{
    //    glass = !glass;
    //    magnifyingGlass.blocksRaycasts = glass;

    //    if (currentFabric != null) // Disable buttons and outline
    //    {
    //        currentFabric.outline.SetActive(false);
    //        currentFabric = null;
    //        // for (int i = 0; i < interactableButtons.Length; i++)
    //            // interactableButtons[i].interactable = false;
    //    }
    //}
}
