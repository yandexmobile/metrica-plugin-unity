/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
#if UNITY_IOS || UNITY_IPHONE
using System;
using System.Runtime.InteropServices;
using UnityEngine.iOS;
#endif

public class IdfaSceneManager : BaseSceneManager
{
    private readonly PopUp _popupWindow = new PopUp();

    protected override void Content()
    {
        _popupWindow.OnGUI();

#if UNITY_IOS || UNITY_IPHONE
        GUILayout.Label("IDFA [unity]");
        GUILayout.Label(Device.advertisingIdentifier);
        GUILayout.Label("IDFA [native]");
        NotEditorLabel(ymm_sample_getIdfa);
        GUILayout.Label("Tracking enabled");
        NotEditorLabel(() => ymm_sample_isAdvertisingTrackingEnabled() ? "True" : "False");
        GUILayout.Label("Tracking authorization status");
        NotEditorLabel(() =>
            ((YandexAppMetricaRequestTrackingStatus)ymm_sample_trackingAuthorizationStatus()).ToString());

        Button("Request IDFA",
            () => AppMetrica.Instance.RequestTrackingAuthorization(status =>
                _popupWindow.ShowPopup(status.ToString())));
#else
        GUILayout.Label("Supported only on iOS");
#endif
    }

    protected override void BottomContent()
    {
        LoadSceneButton("Back To Main Scene", "MainScene");
    }

#if UNITY_IOS || UNITY_IPHONE
    private delegate void YMMSampleTequestTrackingAuthorizationDelegate(IntPtr actionPtr, string status);

    [DllImport("__Internal")]
    private static extern string ymm_sample_getIdfa();

    [DllImport("__Internal")]
    private static extern bool ymm_sample_isAdvertisingTrackingEnabled();

    [DllImport("__Internal")]
    private static extern long ymm_sample_trackingAuthorizationStatus();
#endif
}
