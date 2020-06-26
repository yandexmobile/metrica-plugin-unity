/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

/// <summary>
/// Postprocess build player for Metrica.
/// See https://bitbucket.org/Unity-Technologies/iosnativecodesamples/src/ae6a0a2c02363d35f954d244a6eec91c0e0bf194/NativeIntegration/Misc/UpdateXcodeProject/
/// </summary>

public class PostprocessBuildPlayerAppMetrica
{
    private static readonly string[] StrongFrameworks = {
#if APP_METRICA_ADD_IAD_FRAMEWORK
        "iAd",
#endif
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
#if UNITY_IPHONE || UNITY_IOS
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        var expectedTarget = BuildTarget.iOS;
#else
        var expectedTarget = BuildTarget.iPhone;
#endif
        if (buildTarget == expectedTarget) {
            var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            var project = new PBXProject ();
            project.ReadFromString (File.ReadAllText (projectPath));

#if UNITY_2019_3_OR_NEWER
            var target = project.GetUnityFrameworkTargetGuid();
#else
            var target = project.TargetGuidByName ("Unity-iPhone");
#endif

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
#endif
    }
}
