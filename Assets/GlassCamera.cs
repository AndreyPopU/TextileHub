using UnityEngine;

public class GlassCamera : MonoBehaviour
{
    public Transform glass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, glass.position);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }
}
