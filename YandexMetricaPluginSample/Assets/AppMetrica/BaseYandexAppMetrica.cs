/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;
using System.Collections.Generic;

public abstract class BaseYandexAppMetrica : IYandexAppMetrica
{
    public event ConfigUpdateHandler OnActivation;

    public YandexAppMetricaConfig? ActivationConfig { get; private set; }

    public virtual void ActivateWithConfiguration(YandexAppMetricaConfig config)
    {
        UpdateConfiguration(config);
    }

    public abstract void ResumeSession();

    public abstract void PauseSession();

    public abstract void ReportEvent(string message);

    public abstract void ReportEvent(string message, IDictionary<string, object> parameters);

    public abstract void ReportEvent(string message, string json);

    [Obsolete("Use the ReportError(Exception exception, string condition) instead.")]
    public abstract void ReportError(string condition, string stackTrace);

    [Obsolete("Use the ReportError(string groupIdentifier, string condition, Exception exception) instead.")]
    public abstract void ReportError(string groupIdentifier, string condition, string stackTrace);

    public abstract void ReportError(string groupIdentifier, string condition, Exception exception);

    public abstract void ReportError(
        string groupIdentifier,
        string condition,
        YandexAppMetricaErrorDetails errorDetails
    );

    public abstract void ReportError(Exception exception, string condition);

    public abstract void ReportError(YandexAppMetricaErrorDetails errorDetails, string condition);

    public abstract void ReportUnhandledException(Exception exception);

    public abstract void ReportUnhandledException(YandexAppMetricaErrorDetails errorDetails);

    public abstract void ReportErrorFromLogCallback(string condition, string stackTrace);

    public abstract void SetLocationTracking(bool enabled);

    public abstract void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates);

    public abstract string LibraryVersion { get; }

    public abstract int LibraryApiLevel { get; }

    public abstract void SetUserProfileID(string userProfileID);

    public abstract void ReportUserProfile(YandexAppMetricaUserProfile userProfile);

    public abstract void ReportRevenue(YandexAppMetricaRevenue revenue);

    public abstract void SetStatisticsSending(bool enabled);

    public abstract void SendEventsBuffer();

    public abstract void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action);

    public abstract void ReportReferralUrl(string referralUrl);

    public abstract void ReportAppOpen(string deeplink);

    public abstract void PutErrorEnvironmentValue(string key, string value);

    public abstract void RequestTrackingAuthorization(Action<YandexAppMetricaRequestTrackingStatus> action);

    private void UpdateConfiguration(YandexAppMetricaConfig config)
    {
        ActivationConfig = config;
        ConfigUpdateHandler receiver = OnActivation;
        if (receiver != null)
        {
            receiver.Invoke(config);
        }
    }
}
