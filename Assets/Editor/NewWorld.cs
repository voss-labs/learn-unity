using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class NewWorld : EditorWindow
{
    const string Worlds = "Assets/Worlds";
    const string Template = "Starter";

    string worldName = "";

    [MenuItem("Learn Unity/New world")]
    static void Open()
    {
        var window = GetWindow<NewWorld>(true, "New world");
        window.minSize = window.maxSize = new Vector2(360f, 110f);
    }

    void OnGUI()
    {
        worldName = EditorGUILayout.TextField("Name", worldName);
        var problem = Problem(worldName);
        EditorGUILayout.HelpBox(problem ?? $"Creates {Worlds}/{worldName}", problem == null ? MessageType.Info : MessageType.Warning);
        using (new EditorGUI.DisabledScope(problem != null))
            if (GUILayout.Button("Create"))
            {
                Create(worldName);
                Close();
            }
    }

    static string Problem(string name)
    {
        if (!Regex.IsMatch(name, "^[A-Z][A-Za-z0-9]*$"))
            return "Use letters and numbers only, starting with a capital: Treehouse";
        return AssetDatabase.IsValidFolder($"{Worlds}/{name}") ? $"{name} already exists" : null;
    }

    static void Create(string world)
    {
        var folder = $"{Worlds}/{world}";
        AssetDatabase.CreateFolder(Worlds, world);
        foreach (var source in Directory.GetFiles($"{Worlds}/{Template}").Where(IsCopied))
            AssetDatabase.CopyAsset(source, $"{folder}/{Path.GetFileName(source).Replace(Template, world)}");
        File.WriteAllText($"{folder}/README.md", ReadmeFor(world));
        AssetDatabase.ImportAsset($"{folder}/README.md");

        var path = $"{folder}/{world}.unity";
        var scene = EditorSceneManager.OpenScene(path);
        foreach (var renderer in scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Renderer>(true)))
            renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>($"{folder}/{renderer.sharedMaterial.name}.mat");
        FindAnyObjectByType<Volume>().sharedProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>($"{folder}/{world}Profile.asset");
        EditorSceneManager.SaveScene(scene);

        EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(path, true)).ToArray();
        AssetDatabase.SaveAssets();
    }

    static bool IsCopied(string file) =>
        file.EndsWith(".unity") || file.EndsWith(".mat") || file.EndsWith(".asset");

    static string ReadmeFor(string world) => $@"# {world}

One or two sentences on what this world is.

Made by your name.

## How it was built

The trick you worked out, so that the next person can learn it.

## Credit

Every model, texture and sound you used, with a link and its license.
";
}
