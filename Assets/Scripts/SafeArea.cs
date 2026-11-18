using UnityEngine;

public class SafeArea : MonoBehaviour
{
    void Start()
    {
        var rt = (RectTransform)transform;
        var sa = Screen.safeArea;
        Vector2 min = sa.position, max = sa.position + sa.size;
        min.x /= Screen.width;  min.y /= Screen.height;
        max.x /= Screen.width;  max.y /= Screen.height;
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
    }
}