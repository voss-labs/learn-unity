using UnityEngine;

static class Hud
{
    const float ReferenceHeight = 720f;
    const float Margin = 16f;
    const string Help = "Click to walk   WASD or arrows move   Shift run   Mouse look   Number keys jump   Esc release";

    static Texture2D panel;
    static GUIStyle title, body;

    public static void Draw(string label, bool showHelp)
    {
        var scale = Screen.height / ReferenceHeight;
        GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
        if (panel == null)
            CreateStyles();
        if (label != null)
            Label(label, title, Margin);
        if (showHelp)
            Label(Help, body, ReferenceHeight - Margin - body.CalcSize(new GUIContent(Help)).y);
    }

    static void Label(string text, GUIStyle style, float y)
    {
        var content = new GUIContent(text);
        GUI.Label(new Rect(new Vector2(Margin, y), style.CalcSize(content)), content, style);
    }

    static void CreateStyles()
    {
        panel = new Texture2D(1, 1);
        panel.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.8f));
        panel.Apply();
        title = Style(24, FontStyle.Bold);
        body = Style(15, FontStyle.Normal);
    }

    static GUIStyle Style(int size, FontStyle fontStyle)
    {
        return new GUIStyle(GUI.skin.label)
        {
            fontSize = size,
            fontStyle = fontStyle,
            wordWrap = false,
            padding = new RectOffset(10, 10, 6, 6),
            normal = { textColor = new Color(0.1f, 0.12f, 0.12f), background = panel },
        };
    }
}
