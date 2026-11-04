using UnityEngine;

static class Hud
{
    const float DesktopShortSide = 720f;
    const float PhoneShortSide = 400f;
    const float Margin = 16f;
    const string KeyboardHelp = "Click to walk   WASD or arrows move   Shift run   Mouse look   Number keys jump   Esc release";
    const string TouchHelp = "Left thumb moves   Right thumb looks";
    const string NextPlace = "Next place";
    const int DiscSize = 128;

    static readonly Color Ink = new(0.941f, 0.933f, 0.961f);
    static readonly Color Panel = new(0.02f, 0.02f, 0.063f, 0.72f);
    static readonly Color StickRing = new(0.941f, 0.933f, 0.961f, 0.2f);
    static readonly Color StickKnob = new(0.302f, 0.486f, 1f, 0.9f);

    static Texture2D panel, ring, knob;
    static GUIStyle title, body, button;
    static float scale = 1f;

    public static Rect NextPlaceButton { get; private set; }

    public static void Draw(string label, bool cursorFree, TouchControls touch)
    {
        var touchFirst = Application.isMobilePlatform || touch.Used;
        scale = Mathf.Min(Screen.width, Screen.height) / (touchFirst ? PhoneShortSide : DesktopShortSide);
        GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
        if (panel == null)
            CreateStyles();

        if (label != null)
            Label(label, title, Margin);
        var help = touchFirst ? (touch.Used ? null : TouchHelp) : (cursorFree ? KeyboardHelp : null);
        if (help != null)
            Label(help, body, Screen.height / scale - Margin - body.CalcSize(new GUIContent(help)).y);
        if (touchFirst)
        {
            DrawNextPlace();
            DrawStick(touch);
        }
    }

    static void Label(string text, GUIStyle style, float y)
    {
        var content = new GUIContent(text);
        GUI.Label(new Rect(new Vector2(Margin, y), style.CalcSize(content)), content, style);
    }

    static void DrawNextPlace()
    {
        var content = new GUIContent(NextPlace);
        var size = button.CalcSize(content);
        var rect = new Rect(Screen.width / scale - Margin - size.x, Margin, size.x, size.y);
        GUI.Label(rect, content, button);
        NextPlaceButton = new Rect(rect.x * scale, Screen.height - rect.yMax * scale, rect.width * scale, rect.height * scale);
    }

    static void DrawStick(TouchControls touch)
    {
        if (!touch.StickHeld)
            return;
        Disc(touch.StickCentre, touch.StickRadius, ring);
        Disc(touch.StickTip, touch.StickRadius * 0.45f, knob);
    }

    static void Disc(Vector2 screenCentre, float screenRadius, Texture texture)
    {
        var centre = new Vector2(screenCentre.x, Screen.height - screenCentre.y) / scale;
        var radius = screenRadius / scale;
        GUI.DrawTexture(new Rect(centre.x - radius, centre.y - radius, radius * 2f, radius * 2f), texture);
    }

    static void CreateStyles()
    {
        panel = new Texture2D(1, 1);
        panel.SetPixel(0, 0, Panel);
        panel.Apply();
        ring = MakeDisc(StickRing);
        knob = MakeDisc(StickKnob);
        title = Style(24, FontStyle.Bold);
        body = Style(15, FontStyle.Normal);
        button = Style(17, FontStyle.Bold);
        button.padding = new RectOffset(16, 16, 12, 12);
    }

    static Texture2D MakeDisc(Color color)
    {
        var disc = new Texture2D(DiscSize, DiscSize, TextureFormat.RGBA32, false);
        var middle = (DiscSize - 1) / 2f;
        for (var y = 0; y < DiscSize; y++)
            for (var x = 0; x < DiscSize; x++)
            {
                var edge = Mathf.Clamp01(middle - Vector2.Distance(new Vector2(x, y), new Vector2(middle, middle)));
                disc.SetPixel(x, y, new Color(color.r, color.g, color.b, color.a * edge));
            }
        disc.Apply();
        return disc;
    }

    static GUIStyle Style(int size, FontStyle fontStyle)
    {
        return new GUIStyle(GUI.skin.label)
        {
            fontSize = size,
            fontStyle = fontStyle,
            wordWrap = false,
            padding = new RectOffset(10, 10, 6, 6),
            normal = { textColor = Ink, background = panel },
        };
    }
}
