using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CandleLight : MonoBehaviour
{
    public Sprite glowOverride;           // circle_05
    public Color tint = new Color(0.91f, 0.54f, 0.17f); // candle orange #E8892B
    public float baseIntensity = 0.40f;   // average glow alpha
    public float flutter = 0.15f;         // how far the intensity swings
    public float speed = 2.0f;            // flicker speed
    public float scaleAmount = 0.08f;     // glow size breathes with intensity
    public RectTransform flame;           // flame image that wobbles too

    Image img;
    Vector3 glowBase, flameBase;
    float seed;

    void Awake()
    {
        img = GetComponent<Image>();
        img.raycastTarget = false;                 // never block taps
        if (glowOverride != null) img.sprite = glowOverride;
        glowBase = transform.localScale;
        if (flame) flameBase = flame.localScale;
        seed = Random.value * 100f;
    }

    void Update()
    {
        float t = Time.unscaledTime * speed;       // unscaled: keeps flickering while paused

        // slow drift + fast flutter
        float n = Mathf.PerlinNoise(seed, t) * 0.65f
                + Mathf.PerlinNoise(seed + 10f, t * 3.3f) * 0.35f;      // 0..1

        // occasional draught: a brief dip in brightness
        float gust = Mathf.PerlinNoise(seed + 50f, Time.unscaledTime * 0.5f);
        float dip = 1f - Mathf.SmoothStep(0.72f, 0.95f, gust) * 0.45f;

        float k = (baseIntensity + (n - 0.5f) * 2f * flutter) * dip;
        var c = tint; c.a = Mathf.Clamp01(k);
        img.color = c;

        float s = 1f + (n - 0.5f) * 2f * scaleAmount;
        transform.localScale = glowBase * s;
        if (flame) flame.localScale = new Vector3(flameBase.x * (0.92f + 0.16f * n),
                                                  flameBase.y * (0.88f + 0.24f * n), 1f);
    }
}