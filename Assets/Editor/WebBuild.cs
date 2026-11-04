using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class WebBuild
{
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

        var projectName = PlayerSettings.productName;
        PlayerSettings.productName = world;
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { scene },
            locationPathName = $"site/{world.ToLowerInvariant()}",
            target = BuildTarget.WebGL,
            options = BuildOptions.None,
        });
        PlayerSettings.productName = projectName;

        Debug.Log($"Web build of {world}: {report.summary.result}, {report.summary.totalSize / 1000000} MB, {report.summary.totalErrors} errors");
        if (report.summary.result != BuildResult.Succeeded && Application.isBatchMode)
            EditorApplication.Exit(1);
    }
}
