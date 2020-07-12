using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
 
public class IosAppendExceptionFix : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
 
    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.iOS)
            return;
 
        var buildPath = report.summary.outputPath;
        var catalogPath = Path.Combine(buildPath, "Unity-iPhone", "Images.xcassets");
        if (!Directory.Exists(catalogPath)) {
            Debug.LogError("Could not find assets catalog at path: " + catalogPath);
            return;
        }
 
        var launchimagePath = Path.Combine(catalogPath, "LaunchImage.launchimage");
        if (Directory.Exists(launchimagePath)) {
            // Nothing to do
            return;
        }
 
        Debug.Log("Created LaunchImage.launchimage directory to work around exception during Unity iOS append builds");
        Directory.CreateDirectory(launchimagePath);
    }
}
