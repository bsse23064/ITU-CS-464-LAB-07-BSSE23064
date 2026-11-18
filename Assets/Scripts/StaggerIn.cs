using UnityEngine;

public class StaggerIn : MonoBehaviour
{
    public RectTransform[] items;
    public float delay = 0.07f;

    void OnEnable()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].localScale = Vector3.zero;
            this.ScaleTo(items[i], 1f, 0.35f, Easing.OutBack, null, 0.15f + i * delay);
        }
    }
}