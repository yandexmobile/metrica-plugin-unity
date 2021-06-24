/*
 * Version for Unity
 * © 2021 YANDEX
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

public class PostprocessBuildPlayerAppMetricaSample
{
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
            var infoPlist = new PlistDocument ();
            infoPlist.ReadFromFile (path + "/Info.plist");
            infoPlist.root.SetString ("NSUserTrackingUsageDescription", "The IDFA is needed to be displayed in the app");
            infoPlist.WriteToFile (path + "/Info.plist");
        }
#endif
    }
}
