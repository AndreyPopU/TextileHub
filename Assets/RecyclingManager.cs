using UnityEngine;

public class RecyclingManager : MonoBehaviour
{
    public CanvasGroup magnifyingGlass;
    public bool glass;
    public MaterialFabric currentFabric;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("Recycled");
            if (currentFabric != null) Destroy(currentFabric.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            print("Destroyed");
            if (currentFabric != null) Destroy(currentFabric.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            glass = !glass;
            magnifyingGlass.blocksRaycasts = glass;

            if (currentFabric != null)
            {
                currentFabric.outline.SetActive(false);
                currentFabric = null;
            }
        }

        if (glass) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world coordinates
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorld2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // Raycast at the mouse position
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Hit 2D object: " + hit.collider.name);

                if (hit.collider.transform.TryGetComponent(out MaterialFabric fabric))
                {
                    if (fabric.visible)
                    {
                        Debug.Log("Fabric is visible");
                        if (currentFabric != null)
                        {
                            currentFabric.outline.SetActive(false);
                            currentFabric = null;
                        }
                        currentFabric = fabric;
                        fabric.outline.SetActive(true);
                    }
                }
            }
        }
    }
}
