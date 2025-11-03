using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialFabric : MonoBehaviour
{
    public bool visible;
    [HideInInspector] public GameObject outline;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        outline = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        visible = spriteRenderer.isVisible;
    }
}
