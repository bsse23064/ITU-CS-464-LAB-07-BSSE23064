using UnityEngine;
using UnityEngine.UI;

public class LockedLevel : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => this.Shake((RectTransform)transform, 20f, 0.3f));
    }
}