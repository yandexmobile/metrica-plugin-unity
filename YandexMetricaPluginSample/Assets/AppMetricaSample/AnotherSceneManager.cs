/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System.Collections.Generic;
using UnityEngine;
using YMMJSONUtils;

public class AnotherSceneManager : BaseSceneManager
{
    private readonly Dictionary<string, object> _eventParameters = new Dictionary<string, object>();
    private readonly PopUp _popupWindow = new PopUp();
    private string _eventValue = "test event";
    private string _key = "key";
    private string _value = "value";

    protected override void Content()
    {
        _popupWindow.OnGUI();

        CustomEventGUI();
        ParamsGUI();

        GUILayout.Label(JSONEncoder.Encode(_eventParameters));
    }

    protected override void BottomContent()
    {
        LoadSceneButton("Back To Main Scene", "MainScene");
    }

    private void CustomEventGUI()
    {
        _eventValue = GUILayout.TextField(_eventValue);
        Button("Report Event", () =>
        {
            AppMetrica.Instance.ReportEvent(_eventValue);
            _popupWindow.ShowPopup("Report: " + _eventValue);
        });
    }

    private void ParamsGUI()
    {
        _key = GUILayout.TextField(_key);
        _value = GUILayout.TextField(_value);
        Button("Add param", () => _eventParameters[_key] = _value);
        Button("Clear params", () => _eventParameters.Clear());
        Button("Report with params", () =>
        {
            AppMetrica.Instance.ReportEvent(_eventValue, _eventParameters);
            _popupWindow.ShowPopup("Report with params");
        });
    }
}
