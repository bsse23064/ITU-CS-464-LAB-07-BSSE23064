using System;
using System.Collections;
using UnityEngine;

public static class Easing
{
    public static float Linear(float t) => t;
    public static float InQuad(float t) => t * t;
    public static float OutQuad(float t) => 1f - (1f - t) * (1f - t);
    public static float InCubic(float t) => t * t * t;
    public static float OutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    public static float InOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;

    public static float OutBack(float t)
    {
        const float c1 = 1.70158f, c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    public static float OutElastic(float t)
    {
        if (t <= 0f) return 0f;
        if (t >= 1f) return 1f;
        const float c4 = 2f * Mathf.PI / 3f;
        return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
    }

    public static float OutBounce(float t)
    {
        const float n1 = 7.5625f, d1 = 2.75f;
        if (t < 1f / d1) return n1 * t * t;
        if (t < 2f / d1) { t -= 1.5f / d1; return n1 * t * t + 0.75f; }
        if (t < 2.5f / d1) { t -= 2.25f / d1; return n1 * t * t + 0.9375f; }
        t -= 2.625f / d1; return n1 * t * t + 0.984375f;
    }
}

public static class Tween
{
    // Core: runs step(easedT) from 0 to 1 over 'dur' seconds (unscaled time, so it works while paused)
    public static Coroutine Run(this MonoBehaviour mb, float dur, Func<float, float> ease,
                                Action<float> step, Action done = null, float delay = 0f)
        => mb.StartCoroutine(Co(dur, ease, step, done, delay));

    static IEnumerator Co(float dur, Func<float, float> ease, Action<float> step, Action done, float delay)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);
        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            step(ease(Mathf.Clamp01(t / dur)));
            yield return null;
        }
        step(ease(1f));
        done?.Invoke();
    }

    public static Coroutine FadeTo(this MonoBehaviour mb, CanvasGroup cg, float to, float dur,
                                   Func<float, float> ease, Action done = null, float delay = 0f)
    {
        float from = cg.alpha;
        return mb.Run(dur, ease, t => cg.alpha = Mathf.LerpUnclamped(from, to, t), done, delay);
    }

    public static Coroutine ScaleTo(this MonoBehaviour mb, Transform tr, float to, float dur,
                                    Func<float, float> ease, Action done = null, float delay = 0f)
    {
        Vector3 from = tr.localScale;
        Vector3 end = Vector3.one * to;
        return mb.Run(dur, ease, t => tr.localScale = Vector3.LerpUnclamped(from, end, t), done, delay);
    }

    public static Coroutine MoveTo(this MonoBehaviour mb, RectTransform rt, Vector2 to, float dur,
                                   Func<float, float> ease, Action done = null, float delay = 0f)
    {
        Vector2 from = rt.anchoredPosition;
        return mb.Run(dur, ease, t => rt.anchoredPosition = Vector2.LerpUnclamped(from, to, t), done, delay);
    }

    // Looping pulse (ping-pong) for "tap to start" and "newest level"
    public static Coroutine Pulse(this MonoBehaviour mb, Transform tr, float maxScale, float dur)
        => mb.StartCoroutine(PulseCo(tr, maxScale, dur));

    static IEnumerator PulseCo(Transform tr, float maxScale, float dur)
    {
        Vector3 a = tr.localScale, b = a * maxScale;
        float t = 0f;
        while (true)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.PingPong(t / dur, 1f);
            tr.localScale = Vector3.Lerp(a, b, Easing.InOutSine(k));
            yield return null;
        }
    }

    // Decaying horizontal shake (locked level feedback)
    public static Coroutine Shake(this MonoBehaviour mb, RectTransform rt, float strength, float dur)
        => mb.StartCoroutine(ShakeCo(rt, strength, dur));

    static IEnumerator ShakeCo(RectTransform rt, float strength, float dur)
    {
        Vector2 origin = rt.anchoredPosition;
        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float decay = 1f - t / dur;
            rt.anchoredPosition = origin + Vector2.right * Mathf.Sin(t * 60f) * strength * decay;
            yield return null;
        }
        rt.anchoredPosition = origin;
    }
}