using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    [SerializeField] ScreenBase[] screens;
    [SerializeField] ScreenBase startScreen;

    readonly Dictionary<string, ScreenBase> lookup = new Dictionary<string, ScreenBase>();
    readonly Stack<ScreenBase> history = new Stack<ScreenBase>();
    ScreenBase current;

    void Awake()
    {
        I = this;
        foreach (var s in screens) { lookup[s.name] = s; s.HideInstant(); }
    }

    void Start()
    {
        current = startScreen;
        current.Show(0);
    }

    // Forward: push current onto the back stack
    public void Go(string name)
    {
        var next = lookup[name];
        history.Push(current);
        if (!next.isOverlay) current.Hide(1);   // overlays leave the screen underneath visible
        current = next;
        current.Show(1);
    }

    // Back: pop the stack
    public void Back()
    {
        if (history.Count == 0) return;
        bool wasOverlay = current.isOverlay;
        current.Hide(-1);
        current = history.Pop();
        if (!wasOverlay) current.Show(-1);      // closing an overlay: screen underneath is still visible
    }

    // Jump to a screen and clear history (e.g. "Main Menu" from Results/Pause)
    public void Root(string name)
    {
        history.Clear();
        var next = lookup[name];
        foreach (var s in screens)
            if (s.gameObject.activeSelf && s != next) s.Hide(-1);
        current = next;
        current.Show(-1);
    }

    // Swap the current screen without growing history (Gameplay -> Results, Retry)
    public void Replace(string name)
    {
        var next = lookup[name];
        foreach (var s in screens)
            if (s.gameObject.activeSelf && s != next) s.Hide(1);
        current = next;
        current.Show(1);
    }
}