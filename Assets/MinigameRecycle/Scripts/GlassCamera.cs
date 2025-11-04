using UnityEngine;

public class GlassCamera : MonoBehaviour
{
    public Transform glass;

    void Update() // Move the camera along with the Magnifying glass
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, glass.position);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }
}
