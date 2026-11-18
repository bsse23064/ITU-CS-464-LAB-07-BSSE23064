using UnityEngine;

public class TitleScreen : ScreenBase
{
    [SerializeField] RectTransform logo, prompt;
    Vector2 logoEnd;
    Coroutine pulse;

    void Awake() { logoEnd = logo.anchoredPosition; }

    void OnEnable()
    {
        logo.anchoredPosition = logoEnd + Vector2.up * 600f;
        this.MoveTo(logo, logoEnd, 0.8f, Easing.OutBounce, null, 0.15f);   // drop in
        prompt.localScale = Vector3.one;
    }

    protected override void OnShown()
    {
        if (pulse != null) StopCoroutine(pulse);
        pulse = this.Pulse(prompt, 1.08f, 0.8f);   // InOutSine loop
    }
}