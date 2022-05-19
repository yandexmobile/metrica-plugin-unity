/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appAppMetrica.Instance_sdk_agreement/
 */

using UnityEngine;

public class MainSceneManager : BaseSceneManager
{
    private readonly PopUp _popupWindow = new PopUp();
    private int _eventCounter = 1;
    private bool _isLocationTracking = true;
    private bool _isStatisticsSending = true;
    private int _testCounter = 1;

    protected override void Content()
    {
        _popupWindow.OnGUI();

        Button("Report Test Event", ReportTestOnClick);
        Button("Send Event Immediately", SendEventImmediatelyOnClick);
        Button("Track Location Enabled: " + _isLocationTracking, TrackLocationEnabledOnClick);
        Button("Send Statistics Enabled: " + _isStatisticsSending, SendStatisticsSendingOnClick);
        Button("Show AppMetrica DeviceID", ShowAppMetricaDeviceIDOnClick);
        Button("Show Library Version", () => _popupWindow.ShowPopup("Version: " + AppMetrica.Instance.LibraryVersion));
        Button("Show Library API Level", () => _popupWindow.ShowPopup("Level: " + AppMetrica.Instance.LibraryApiLevel));

#if UNITY_IOS || UNITY_IPHONE
        LoadSceneButton("[SCENE] IDFA", "IdfaScene");
#endif
        LoadSceneButton("[SCENE] Load", "AnotherScene");
        LoadSceneButton("[SCENE] Crash", "CrashScene");

        Button("Exit", Application.Quit);
    }

    private void ReportTestOnClick()
    {
        string report = "Test Event " + _testCounter++;
        AppMetrica.Instance.ReportEvent(report);
        _popupWindow.ShowPopup("Report: " + report);
    }

    private void SendEventImmediatelyOnClick()
    {
        string report = "Event Immediately " + _eventCounter++;
        AppMetrica.Instance.ReportEvent(report);
        AppMetrica.Instance.SendEventsBuffer();
        _popupWindow.ShowPopup("Report: " + report);
    }

    private void TrackLocationEnabledOnClick()
    {
        _isLocationTracking = !_isLocationTracking;
        AppMetrica.Instance.SetLocationTracking(_isLocationTracking);
    }

    private void SendStatisticsSendingOnClick()
    {
        _isStatisticsSending = !_isStatisticsSending;
        AppMetrica.Instance.SetStatisticsSending(_isStatisticsSending);
    }

    private void ShowAppMetricaDeviceIDOnClick()
    {
        AppMetrica.Instance.RequestAppMetricaDeviceID((deviceId, error) =>
        {
            _popupWindow.ShowPopup(error == null ? "DeviceID: " + deviceId : "Error: " + error);
        });
    }
}
