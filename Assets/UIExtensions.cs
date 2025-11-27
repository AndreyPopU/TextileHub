using UnityEngine;

public static class UIExtensions
{
    public static bool Overlaps(this RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = GetScreenRect(rect1);
        Rect r2 = GetScreenRect(rect2);

        return r1.Overlaps(r2);
    }

    private static Rect GetScreenRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0], corners[2] - corners[0]);
    }
}