/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class BaseYandexAppMetrica : IYandexAppMetrica
{
    private YandexAppMetricaConfig? _metricaConfig;

    public event ConfigUpdateHandler OnActivation;

    public YandexAppMetricaConfig? ActivationConfig {
        get {
            return _metricaConfig;
        }
    }

    public virtual void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
        UpdateConfiguration (config);
    }

    private void UpdateConfiguration (YandexAppMetricaConfig config)
    {
        _metricaConfig = config;
        ConfigUpdateHandler receiver = OnActivation;
        if (receiver != null) {
            receiver (config);
        }
    }

    public abstract void ResumeSession ();

    public abstract void PauseSession ();

    public abstract void ReportEvent (string message);

    public abstract void ReportEvent (string message, Dictionary<string, object> parameters);

    public abstract void ReportError (string condition, string stackTrace);

    public abstract void SetLocationTracking (bool enabled);

    public abstract void SetLocation (YandexAppMetricaConfig.Coordinates? coordinates);

    public abstract string LibraryVersion { get; }

    public abstract int LibraryApiLevel { get; }

    public abstract void SetUserProfileID (string userProfileID);

    public abstract void ReportUserProfile (YandexAppMetricaUserProfile userProfile);

    public abstract void ReportRevenue (YandexAppMetricaRevenue revenue);

    public abstract void SetStatisticsSending (bool enabled);

    public abstract void SendEventsBuffer ();

    public abstract void RequestAppMetricaDeviceID (Action<string, YandexAppMetricaRequestDeviceIDError?> action);

    public abstract void ReportReferralUrl (string referralUrl);
    
    public abstract void ReportAppOpen (string deeplink);
}
