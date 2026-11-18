using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlameFlipbook : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 9f;
    Image img;
    float offset;

    void Awake() { img = GetComponent<Image>(); offset = Random.value * 10f; }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;
        int i = (int)((Time.unscaledTime + offset) * fps) % frames.Length;
        img.sprite = frames[i];
    }
}