/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if UNITY_IOS || UNITY_IPHONE
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;

public class IdfaSceneManager : MonoBehaviour
{
    private delegate void YMMSampleTequestTrackingAuthorizationDelegate (IntPtr actionPtr, string status);

    private PopUp popupWindow = new PopUp ();

    [DllImport ("__Internal")]
    private static extern string ymm_sample_getIdfa ();

    [DllImport ("__Internal")]
    private static extern bool ymm_sample_isAdvertisingTrackingEnabled ();

    [DllImport ("__Internal")]
    private static extern long ymm_sample_trackingAuthorizationStatus ();

    private void OnGUI ()
    {
        popupWindow.onGUI ();

        GUI.contentColor = Color.black;

        GUILayout.Label ("");
        GUILayout.Label ("IDFA [unity]");
        GUILayout.Label (Device.advertisingIdentifier);
        GUILayout.Label ("IDFA [native]");
        GUILayout.Label (ymm_sample_getIdfa ());
        GUILayout.Label ("Tracking enabled");
        GUILayout.Label (ymm_sample_isAdvertisingTrackingEnabled () ? "True" : "False");
        GUILayout.Label ("Tracking authorization status");
        GUILayout.Label (((YandexAppMetricaRequestTrackingStatus)ymm_sample_trackingAuthorizationStatus ()).ToString());

        if (Button ("Request IDFA"))
        {
            AppMetrica.Instance.RequestTrackingAuthorization (status => popupWindow.showPopup (status.ToString()));
        }

        if (Button ("Back To Main Scene"))
        {
            SceneManager.LoadScene ("MainScene");
        }
    }

    private bool Button (string title)
    {
        return GUILayout.Button (title, GUILayout.Width (Screen.width), GUILayout.Height (Screen.height / 13));
    }
}
#endif
