using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AnotherSceneManager : MonoBehaviour
{
    private const int DELAY_BACKGROUND_SEC = 120;
    private const string DEFAULT_EVENT = "test event";
    private const string DEFAULT_KEY = "key";
    private const string DEFAULT_VALUE = "value";
    private const string DEFAULT_ENVIRONMENT = "environment";

    private static string eventValue = DEFAULT_EVENT;
    private static string environmentValue = DEFAULT_ENVIRONMENT;
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
        onEnvironmentGUI ();

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

    void onEnvironmentGUI ()
    {
        environmentValue = GUILayout.TextField (environmentValue);
        if (Button ("Set crash environment")) {
            AppMetrica.Instance.SetEnvironmentValue (DEFAULT_ENVIRONMENT, environmentValue);
            environmentValue = DEFAULT_ENVIRONMENT;
        }
    }
}
