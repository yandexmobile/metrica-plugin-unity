using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections;

/// <summary>
/// Postprocess build player for Metrica.
/// See https://bitbucket.org/Unity-Technologies/iosnativecodesamples/src/ae6a0a2c02363d35f954d244a6eec91c0e0bf194/NativeIntegration/Misc/UpdateXcodeProject/
/// </summary>

public class PostprocessBuildPlayerAppMetrica
{
    private static readonly string[] StrongFrameworks = {
        "SystemConfiguration",
        "UIKit",
        "Foundation",
        "CoreTelephony",
        "CoreLocation",
        "CoreGraphics",
        "AdSupport",
        "Security"
    };

    private static readonly string[] WeakFrameworks = {
        "SafariServices"
    };

    private static readonly string[] Libraries = {
        "z",
        "sqlite3",
        "c++"
    };

    private static readonly string[] LDFlags = {
        "-ObjC"
    };

    [PostProcessBuild]
    public static void OnPostprocessBuild (BuildTarget buildTarget, string path)
    {
#if UNITY_5
        var expectedTarget = BuildTarget.iOS;
#else
		var expectedTarget = BuildTarget.iPhone;
#endif
        if (buildTarget == expectedTarget) {
            var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            var project = new PBXProject ();
            project.ReadFromString (File.ReadAllText (projectPath));

            var target = project.TargetGuidByName ("Unity-iPhone");

            foreach (var frameworkName in StrongFrameworks) {
                project.AddFrameworkToProject (target, frameworkName + ".framework", false);
            }
            foreach (var frameworkName in WeakFrameworks) {
                project.AddFrameworkToProject (target, frameworkName + ".framework", true);
            }
            foreach (var flag in LDFlags) {
                project.AddBuildProperty (target, "OTHER_LDFLAGS", flag);
            }
            foreach (var libraryName in Libraries) {
                project.AddBuildProperty (target, "OTHER_LDFLAGS", "-l" + libraryName);
            }

            File.WriteAllText (projectPath, project.WriteToString ());
        }
    }
}
