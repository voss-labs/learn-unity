using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class PhoneSettings
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    static extern int DepthComparisonWorks();
#else
    static int DepthComparisonWorks() => 1;
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Apply()
    {
        if (GraphicsSettings.currentRenderPipeline is not UniversalRenderPipelineAsset pipeline)
            return;
        if (Application.isMobilePlatform)
        {
            pipeline.msaaSampleCount = 2;
            pipeline.shadowDistance = 30f;
            pipeline.shadowCascadeCount = 1;
        }
        if (DepthComparisonWorks() == 0)
            pipeline.shadowDistance = 0f;
    }
}
