/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AnotherSceneManager : MonoBehaviour
{
    private const int DELAY_BACKGROUND_SEC = 120;
    private const string DEFAULT_EVENT = "test event";
    private const string DEFAULT_KEY = "key";
    private const string DEFAULT_VALUE = "value";

    private static string eventValue = DEFAULT_EVENT;
    private Dictionary<string, object> eventParameters = new Dictionary<string, object> ();
    private string key = DEFAULT_KEY;
    private string value = DEFAULT_VALUE;

    private PopUp popupWindow = new PopUp ();

    private void OnGUI ()
    {
        popupWindow.onGUI ();

        GUI.contentColor = Color.black;

        onCustomEventGUI ();
        onParamsGUI ();

        if (Button ("Back To Main Scene")) {
            SceneManager.LoadScene ("MainScene");
        }

        GUILayout.Label (YMMJSONUtils.JSONEncoder.Encode (eventParameters));
    }

    private bool Button (string title)
    {
        return GUILayout.Button (title, GUILayout.Width (Screen.width), GUILayout.Height (Screen.height / 13));
    }

    void onCustomEventGUI ()
    {
        eventValue = GUILayout.TextField (eventValue);
        if (Button ("Report Event")) {
            AppMetrica.Instance.ReportEvent (eventValue);
            popupWindow.showPopup ("Report: " + eventValue);
            eventValue = DEFAULT_EVENT;
        }
    }

    void onParamsGUI ()
    {
        key = GUILayout.TextField (key);
        value = GUILayout.TextField (value);
        if (Button ("Add param")) {
            eventParameters [key] = value;
            key = DEFAULT_KEY;
            value = DEFAULT_VALUE;
        }
        if (Button ("Clear params")) {
            eventParameters.Clear ();
        }
        if (Button ("Report with params")) {
            AppMetrica.Instance.ReportEvent (eventValue, eventParameters);
            popupWindow.showPopup ("Report with params");
            eventParameters.Clear ();
        }
    }
}
