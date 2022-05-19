/*
 * Version for Unity
 * © 2015-2022 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseSceneManager : MonoBehaviour
{
    private Vector2 _scrollPosition;

    protected virtual void Update()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }

        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Moved)
        {
            _scrollPosition.y += touch.deltaPosition.y;
        }
    }

    protected virtual void OnGUI()
    {
        ConfigureGUISkins();
        GUILayout.BeginArea(SafeAreaRect());
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        Content();
        GUILayout.EndScrollView();
        BottomContent();
        GUILayout.EndArea();
    }

    protected virtual void ConfigureGUISkins()
    {
        GUI.skin.button.fontSize = 40;
        GUI.skin.label.fontSize = 40;
        GUI.skin.textField.fontSize = 35;

        GUIStyleState labelStyle = new GUIStyleState { textColor = Color.black };
        GUI.skin.label.normal = labelStyle;
        GUI.skin.label.hover = labelStyle;
    }

    protected virtual void BottomContent()
    {
    }

    protected abstract void Content();

    protected void Button(string text, Action onClick)
    {
        if (GUILayout.Button(text, GUILayout.Height(50 * Screen.dpi / 160)))
        {
            onClick();
        }
    }

    protected void LoadSceneButton(string text, string scene)
    {
        Button(text, () => SceneManager.LoadScene(scene));
    }

    protected void NotEditorLabel(Func<string> lazyText)
    {
        GUILayout.Label(Application.isEditor ? "This label not supported in Editor mode" : lazyText());
    }

    private static Rect SafeAreaRect()
    {
#if UNITY_ANDROID
        switch (Screen.orientation)
        {
            case ScreenOrientation.Portrait:
                return new Rect(Screen.width - Screen.safeArea.width, Screen.height - Screen.safeArea.height,
                    Screen.safeArea.width, Screen.safeArea.height);
            case ScreenOrientation.PortraitUpsideDown:
                return new Rect(0, 0, Screen.safeArea.width, Screen.safeArea.height);
            case ScreenOrientation.Unknown:
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight:
            case ScreenOrientation.AutoRotation:
            default:
                return Screen.safeArea;
        }
#else
        return Screen.safeArea;
#endif
    }
}
