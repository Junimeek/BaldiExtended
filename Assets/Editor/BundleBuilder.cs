using UnityEngine;
using UnityEditor;

public class BundleBuilder
{
    [MenuItem("Tools/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        string streamingPath = Application.streamingAssetsPath;
        try
        {
            BuildPipeline.BuildAssetBundles(streamingPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}
