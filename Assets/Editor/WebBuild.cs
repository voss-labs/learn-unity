using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class WebBuild
{
    const string PhoneBuildRoot = "PhoneBuild";
    const int DefaultPhoneTextureLimit = 512;

    static readonly Dictionary<string, int> PhoneTextureLimits = new() { { "Campus", 1024 } };

    [MenuItem("Learn Unity/Build this world for the web")]
    static void BuildOpenWorld()
    {
        Build(Path.GetFileNameWithoutExtension(EditorSceneManager.GetActiveScene().path));
    }

    public static void Run()
    {
        var args = Environment.GetCommandLineArgs();
        var at = Array.IndexOf(args, "-world");
        if (at < 0 || at + 1 >= args.Length)
            throw new ArgumentException("Pass -world <Name>, for example -world Starter.");
        Build(args[at + 1]);
    }

    static void Build(string world)
    {
        var scene = $"Assets/Worlds/{world}/{world}.unity";
        if (!File.Exists(scene))
            throw new FileNotFoundException($"No world scene at {scene}. A world called X lives at Assets/Worlds/X/X.unity.");

        var name = world.ToLowerInvariant();
        var computer = $"site/{name}";
        var phone = $"{PhoneBuildRoot}/{name}";
        var projectName = PlayerSettings.productName;
        PlayerSettings.productName = world;
        var phoneLimit = PhoneTextureLimits.GetValueOrDefault(world, DefaultPhoneTextureLimit);

        var succeeded = Variant(world, scene, computer, WebGLTextureSubtarget.DXT, 0)
            && Variant(world, scene, phone, WebGLTextureSubtarget.ASTC, phoneLimit)
            && AddPhoneData(world, name, computer, phone);

        PlayerSettings.productName = projectName;
        AssetDatabase.SaveAssets();
        if (!succeeded && Application.isBatchMode)
            EditorApplication.Exit(1);
    }

    static bool Variant(string world, string scene, string output, WebGLTextureSubtarget textures, int textureLimit)
    {
        EditorUserBuildSettings.overrideMaxTextureSize = textureLimit;
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { scene },
            locationPathName = output,
            target = BuildTarget.WebGL,
            subtarget = (int)textures,
            options = BuildOptions.None,
        });
        EditorUserBuildSettings.overrideMaxTextureSize = 0;
        Debug.Log($"Web build of {world} ({textures}): {report.summary.result}, {report.summary.totalSize / 1000000} MB, {report.summary.totalErrors} errors");
        return report.summary.result == BuildResult.Succeeded;
    }

    static bool AddPhoneData(string world, string name, string computer, string phone)
    {
        var code = $"Build/{name}.wasm.unityweb";
        if (!File.ReadAllBytes($"{computer}/{code}").SequenceEqual(File.ReadAllBytes($"{phone}/{code}")))
        {
            Debug.LogError($"The computer and phone builds of {world} have different code, so they cannot share one page. Build again.");
            return false;
        }
        File.Copy($"{phone}/Build/{name}.data.unityweb", $"{computer}/Build/{name}.mobile.data.unityweb", true);
        Directory.Delete(PhoneBuildRoot, true);
        return true;
    }
}
