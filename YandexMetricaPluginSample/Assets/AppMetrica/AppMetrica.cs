/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

// Uncomment the following line to disable location tracking
// #define APP_METRICA_TRACK_LOCATION_DISABLED
// or just add APP_METRICA_TRACK_LOCATION_DISABLED into
// Player Settings -> Other Settings -> Scripting Define Symbols

using UnityEngine;
using System.Collections;

public class AppMetrica : MonoBehaviour
{
    public const string VERSION = "4.2.0";

    [SerializeField]
    private string ApiKey;

    [SerializeField]
    private bool ExceptionsReporting = true;

    [SerializeField]
    private uint SessionTimeoutSec = 10;

    [SerializeField]
    private bool LocationTracking = true;

    [SerializeField]
    private bool Logs = true;

    [SerializeField]
    private bool HandleFirstActivationAsUpdate = false;

    [SerializeField]
    private bool StatisticsSending = true;

    private static bool _isInitialized = false;
    private bool _actualPauseStatus = false;

    private static IYandexAppMetrica _metrica = null;
    private static object syncRoot = new Object ();

    public static IYandexAppMetrica Instance {
        get {
            if (_metrica == null) {
                lock (syncRoot) {
#if UNITY_IPHONE || UNITY_IOS
                    if (_metrica == null && Application.platform == RuntimePlatform.IPhonePlayer) {
                        _metrica = new YandexAppMetricaIOS ();
                    }
#elif UNITY_ANDROID
					if (_metrica == null && Application.platform == RuntimePlatform.Android) {
						_metrica = new YandexAppMetricaAndroid();
					}
#endif
                    if (_metrica == null) {
                        _metrica = new YandexAppMetricaDummy ();
                    }
                }
            }
            return _metrica;
        }
    }

    void SetupMetrica ()
    {
        var configuration = new YandexAppMetricaConfig (ApiKey) {
            SessionTimeout = (int)SessionTimeoutSec,
            Logs = Logs,
            HandleFirstActivationAsUpdate = HandleFirstActivationAsUpdate,
            StatisticsSending = StatisticsSending,
        };

#if !APP_METRICA_TRACK_LOCATION_DISABLED
        configuration.LocationTracking = LocationTracking;
        if (LocationTracking) {
            Input.location.Start ();
        }
#else
        configuration.LocationTracking = false;
#endif

        Instance.ActivateWithConfiguration (configuration);
    }

    private void Awake ()
    {
        if (!_isInitialized) {
            _isInitialized = true;
            DontDestroyOnLoad (this.gameObject);
            SetupMetrica ();
        } else {
            Destroy (this.gameObject);
        }
    }

    private void Start ()
    {
        Instance.ResumeSession ();
    }

    private void OnEnable ()
    {
        if (ExceptionsReporting) {
#if UNITY_5 || UNITY_5_3_OR_NEWER
            Application.logMessageReceived += HandleLog;
#else
			Application.RegisterLogCallback(HandleLog);
#endif
        }
    }

    private void OnDisable ()
    {
        if (ExceptionsReporting) {
#if UNITY_5 || UNITY_5_3_OR_NEWER
            Application.logMessageReceived -= HandleLog;
#else
			Application.RegisterLogCallback(null);
#endif
        }
    }

    private void OnApplicationPause (bool pauseStatus)
    {
        if (_actualPauseStatus != pauseStatus) {
            _actualPauseStatus = pauseStatus;
            if (pauseStatus) {
                Instance.PauseSession ();
            } else {
                Instance.ResumeSession ();
            }
        }
    }

    private void HandleLog (string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception) {
            Instance.ReportError (condition, condition, stackTrace);
        }
    }

}
