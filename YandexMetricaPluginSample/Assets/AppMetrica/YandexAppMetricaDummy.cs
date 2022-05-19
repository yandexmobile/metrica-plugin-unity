/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;
using System.Collections.Generic;

public class YandexAppMetricaDummy : BaseYandexAppMetrica
{
    #region IYandexAppMetrica implementation

    public override void ActivateWithConfiguration(YandexAppMetricaConfig config)
    {
    }

    public override void ResumeSession()
    {
    }

    public override void PauseSession()
    {
    }

    public override void ReportEvent(string message)
    {
    }

    public override void ReportEvent(string message, IDictionary<string, object> parameters)
    {
    }

    public override void ReportEvent(string message, string json)
    {
    }

    public override void ReportError(string condition, string stackTrace)
    {
    }

    public override void ReportError(string groupIdentifier, string condition, string stackTrace)
    {
    }

    public override void ReportError(string groupIdentifier, string condition, Exception exception)
    {
    }

    public override void ReportError(
        string groupIdentifier,
        string condition,
        YandexAppMetricaErrorDetails errorDetails)
    {
    }

    public override void ReportError(Exception exception, string condition)
    {
    }

    public override void ReportError(YandexAppMetricaErrorDetails errorDetails, string condition)
    {
    }

    public override void ReportUnhandledException(Exception exception)
    {
    }

    public override void ReportUnhandledException(YandexAppMetricaErrorDetails errorDetails)
    {
    }

    public override void ReportErrorFromLogCallback(string condition, string stackTrace)
    {
    }

    public override void SetLocationTracking(bool enabled)
    {
    }

    public override void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates)
    {
    }

    public override string LibraryVersion
    {
        get
        {
            return default(string);
        }
    }

    public override int LibraryApiLevel
    {
        get
        {
            return default(int);
        }
    }

    public override void SetUserProfileID(string userProfileID)
    {
    }

    public override void ReportUserProfile(YandexAppMetricaUserProfile userProfile)
    {
    }

    public override void ReportRevenue(YandexAppMetricaRevenue revenue)
    {
    }

    public override void SetStatisticsSending(bool enabled)
    {
    }

    public override void SendEventsBuffer()
    {
    }

    public override void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action)
    {
    }

    public override void ReportReferralUrl(string referralUrl)
    {
    }

    public override void ReportAppOpen(string deeplink)
    {
    }

    public override void PutErrorEnvironmentValue(string key, string value)
    {
    }

    public override void RequestTrackingAuthorization(Action<YandexAppMetricaRequestTrackingStatus> action)
    {
    }

    #endregion
}
