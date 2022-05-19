/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if UNITY_IPHONE || UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

/// <summary>
///     Postprocess build player for Metrica.
///     See
///     https://bitbucket.org/Unity-Technologies/iosnativecodesamples/src/ae6a0a2c02363d35f954d244a6eec91c0e0bf194/NativeIntegration/Misc/UpdateXcodeProject/
/// </summary>
public class PostprocessBuildPlayerAppMetrica
{
    private static readonly string[] s_strongFrameworks =
    {
#if APP_METRICA_ADD_IAD_FRAMEWORK
        "iAd",
#endif
        "SystemConfiguration", "UIKit", "Foundation", "CoreTelephony", "CoreLocation", "CoreGraphics", "AdSupport",
        "Security", "WebKit"
    };

    private static readonly string[] s_weakFrameworks = { "SafariServices" };

    private static readonly string[] s_libraries = { "z", "sqlite3", "c++" };

    private static readonly string[] s_ldFlags = { "-ObjC" };

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        BuildTarget expectedTarget = BuildTarget.iOS;
#else
        BuildTarget expectedTarget = BuildTarget.iPhone;
#endif
        if (buildTarget == expectedTarget)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

#if UNITY_2019_3_OR_NEWER
            var target = project.GetUnityFrameworkTargetGuid();
#else
            string target = project.TargetGuidByName("Unity-iPhone");
#endif

            foreach (string frameworkName in s_strongFrameworks)
            {
                project.AddFrameworkToProject(target, frameworkName + ".framework", false);
            }

            foreach (string frameworkName in s_weakFrameworks)
            {
                project.AddFrameworkToProject(target, frameworkName + ".framework", true);
            }

            foreach (string flag in s_ldFlags)
            {
                project.AddBuildProperty(target, "OTHER_LDFLAGS", flag);
            }

            foreach (string libraryName in s_libraries)
            {
                project.AddBuildProperty(target, "OTHER_LDFLAGS", "-l" + libraryName);
            }

            File.WriteAllText(projectPath, project.WriteToString());
        }
    }
}
#endif
