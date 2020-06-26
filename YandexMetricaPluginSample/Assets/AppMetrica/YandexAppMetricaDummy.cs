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

public class YandexAppMetricaDummy : BaseYandexAppMetrica
{

    #region IYandexAppMetrica implementation

    public override void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
    }

    public override void ResumeSession ()
    {
    }

    public override void PauseSession ()
    {
    }

    public override void ReportEvent (string message)
    {
    }

    public override void ReportEvent (string message, Dictionary<string, object> parameters)
    {
    }

    public override void ReportError (string condition, string stackTrace)
    {
    }

    public override void SetLocationTracking (bool enabled)
    {
    }

    public override void SetLocation (YandexAppMetricaConfig.Coordinates? coordinates)
    {
    }

    public override string LibraryVersion { get { return default(string); } }

    public override int LibraryApiLevel { get { return default(int); } }

    public override void SetUserProfileID (string userProfileID)
    {
    }

    public override void ReportUserProfile (YandexAppMetricaUserProfile userProfile)
    {
    }

    public override void ReportRevenue (YandexAppMetricaRevenue revenue)
    {
    }

    public override void SetStatisticsSending (bool enabled)
    {
    }

    public override void SendEventsBuffer ()
    {
    }

    public override void RequestAppMetricaDeviceID (Action<string, YandexAppMetricaRequestDeviceIDError?> action)
    {
    }

    public override void ReportReferralUrl(string referralUrl)
    {
    }
    
    public override void ReportAppOpen (string deeplink)
    {
    }

    #endregion

}
