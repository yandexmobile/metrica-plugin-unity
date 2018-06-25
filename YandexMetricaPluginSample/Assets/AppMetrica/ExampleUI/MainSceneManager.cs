/*
 * Version for Unity
 * © 2015-2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    private GameObject nullGameObject = null;

    private static bool isLocationTracking = true;
    private PopUp popupWindow = new PopUp ();
    private static int counter = 1;

    private void InitGUI ()
    {
        GUI.skin.button.fontSize = 40;
        GUI.skin.textField.fontSize = 35;
        GUI.contentColor = Color.white;
        GUI.skin.label.fontSize = 40;
    }

    private void OnGUI ()
    {
        InitGUI ();
        popupWindow.onGUI ();

        var metrica = AppMetrica.Instance;
        if (Button ("Report Test")) {
            string report = "Test" + counter++;
            metrica.ReportEvent (report);
            popupWindow.showPopup ("Report: " + report);
        }
        if (Button ("Track Location Enabled: " + isLocationTracking)) {
            isLocationTracking = !isLocationTracking;
            metrica.SetLocationTracking (isLocationTracking);
        }
        if (Button ("[CRASH] NullReference")) {
            nullGameObject.SendMessage ("");
        }
        if (Button ("LOG Library Version")) {
            popupWindow.showPopup ("Version: " + metrica.LibraryVersion);
        }
        if (Button ("LOG Library API Level")) {
            popupWindow.showPopup ("Level: " + metrica.LibraryApiLevel);
        }
        if (Button ("[SCENE] Load")) {
            SceneManager.LoadScene ("AnotherScene");
        }

        if (Button ("Exit")) {
            Application.Quit ();
        }
    }

    private bool Button (string title)
    {
        return GUILayout.Button (title, GUILayout.Width (Screen.width), GUILayout.Height (Screen.height / 10));
    }
}
