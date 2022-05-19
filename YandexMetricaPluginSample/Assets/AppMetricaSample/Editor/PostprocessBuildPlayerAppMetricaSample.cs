/*
 * Version for Unity
 * © 2021 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if UNITY_IPHONE || UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class PostprocessBuildPlayerAppMetricaSample
{
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
            PlistDocument infoPlist = new PlistDocument();
            infoPlist.ReadFromFile(path + "/Info.plist");
            infoPlist.root.SetString("NSUserTrackingUsageDescription", "The IDFA is needed to be displayed in the app");
            infoPlist.WriteToFile(path + "/Info.plist");
        }
    }
}
#endif
