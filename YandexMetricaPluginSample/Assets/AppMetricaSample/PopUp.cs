/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;

public class PopUp
{
    private bool _isPopupNeeded;
    private string _popupText;

    public void OnGUI()
    {
        if (_isPopupNeeded)
        {
            GUILayout.Window(0, new Rect(Screen.width / 2 - 300, Screen.height / 2 - 65, 600, 150), ShowGUI, "");
        }
    }

    public void ShowPopup(string text)
    {
        _popupText = text;
        _isPopupNeeded = true;
    }

    private void ShowGUI(int windowID)
    {
        GUILayout.Label(_popupText);
        if (GUILayout.Button("OK"))
        {
            _isPopupNeeded = false;
        }
    }
}
