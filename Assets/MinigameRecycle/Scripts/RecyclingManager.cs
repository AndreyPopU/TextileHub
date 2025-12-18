using UnityEngine;
using UnityEngine.UI;

public class RecyclingManager : MonoBehaviour
{
    public CanvasGroup magnifyingGlass;
    public bool glass;
    public MaterialFabric currentFabric;

    public Button[] interactableButtons;

    private Camera mainCamera;

    void Start() => mainCamera = Camera.main;    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SwitchMode();

        if (glass) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world coordinates
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorld2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // Raycast at the mouse position
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

            if (hit.collider != null) // If raycast hits something
            {
                if (hit.collider.transform.TryGetComponent(out MaterialFabric fabric)) // Something that is fabric
                {
                    if (fabric.visible) // If fabric is under magnifying glass
                    {
                        if (currentFabric != null) // Disable the previous fabric if there is one 
                        {
                            currentFabric.outline.SetActive(false);
                            currentFabric = null;
                        }
                        currentFabric = fabric;

                        fabric.outline.SetActive(true);

                        // Enable interactable buttons
                        // for (int i = 0; i < interactableButtons.Length; i++)
                            // interactableButtons[i].interactable = true;
                    }
                }
            }
        }
    }

    public void SwitchMode()
    {
        glass = !glass;
        magnifyingGlass.blocksRaycasts = glass;

        if (currentFabric != null) // Disable buttons and outline
        {
            currentFabric.outline.SetActive(false);
            currentFabric = null;
            // for (int i = 0; i < interactableButtons.Length; i++)
                // interactableButtons[i].interactable = false;
        }
    }

    public void DestroyFabric()
    {
        if (currentFabric != null) Destroy(currentFabric.gameObject);
        print("Destroyed");
    }

    public void RecycleFabric()
    {
        if (currentFabric != null) Destroy(currentFabric.gameObject);
        print("Recycled");
    }
}
