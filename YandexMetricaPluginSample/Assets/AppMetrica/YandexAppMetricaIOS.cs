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
using System.Runtime.InteropServices;
using System;
using System.Globalization;

#if UNITY_IPHONE || UNITY_IOS

public class YandexAppMetricaIOS : BaseYandexAppMetrica
{
    [DllImport ("__Internal")]
    private static extern void ymm_activateWithConfigurationJSON (string configurationJSON);

    [DllImport ("__Internal")]
    private static extern void ymm_reportEvent (string message);

    [DllImport ("__Internal")]
    private static extern void ymm_reportEventWithParameters (string message, string parameters);

    [DllImport ("__Internal")]
    private static extern void ymm_reportError (string condition, string stackTrace);

    [DllImport ("__Internal")]
    private static extern void ymm_setLocationTracking (bool enabled);

    [DllImport ("__Internal")]
    private static extern void ymm_setLocation (double latitude, double longitude);

    [DllImport ("__Internal")]
    private static extern void ymm_resetLocation ();

    [DllImport ("__Internal")]
    private static extern string ymm_getLibraryVersion ();

    [DllImport ("__Internal")]
    private static extern void ymm_setUserProfileID (string userProfileID);

    [DllImport ("__Internal")]
    private static extern void ymm_reportUsertProfileJSON (string userProfileJSON);

    [DllImport ("__Internal")]
    private static extern void ymm_reportRevenueJSON (string revenueJSON);

    [DllImport ("__Internal")]
    private static extern void ymm_setStatisticsSending (bool enabled);

    [DllImport ("__Internal")]
    private static extern void ymm_sendEventsBuffer ();

    [DllImport ("__Internal")]
    private static extern void ymm_requestAppMetricaDeviceID (YMMRequestDeviceIDCallbackDelegate callbackDelegate, IntPtr actionPtr);

    [DllImport("__Internal")]
    private static extern void ymm_reportReferralUrl (string referralUrl);

    [DllImport ("__Internal")]
    private static extern void ymm_reportAppOpen (string deeplink);
    
    private delegate void YMMRequestDeviceIDCallbackDelegate (IntPtr actionPtr, string deviceId, string errorString);

    #region IYandexAppMetrica implementation

    public override void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
        base.ActivateWithConfiguration (config);
        ymm_activateWithConfigurationJSON (JsonStringFromDictionary (config.ToHashtable ()));
    }

    public override void ResumeSession ()
    {
        // It does nothing for iOS
    }

    public override void PauseSession ()
    {
        // It does nothing for iOS
    }

    public override void ReportEvent (string message)
    {
        ymm_reportEvent (message);
    }

    public override void ReportEvent (string message, Dictionary<string, object> parameters)
    {
        ymm_reportEventWithParameters (message, JsonStringFromDictionary (parameters));
    }

    public override void ReportError (string condition, string stackTrace)
    {
        ymm_reportError (condition, stackTrace);
    }

    public override void SetLocationTracking (bool enabled)
    {
        ymm_setLocationTracking (enabled);
    }

    public override void SetLocation (YandexAppMetricaConfig.Coordinates? coordinates)
    {
        if (coordinates.HasValue) {
            ymm_setLocation (coordinates.Value.Latitude, coordinates.Value.Longitude);
        } else {
            ymm_resetLocation ();
        }
    }

    public override int LibraryApiLevel {
        get {
            // Not available for iOS
            return 0;
        }
    }

    public override string LibraryVersion {
        get {
            return ymm_getLibraryVersion ();
        }
    }

    public override void SetUserProfileID (string userProfileID)
    {
        ymm_setUserProfileID (userProfileID);
    }

    public override void ReportUserProfile (YandexAppMetricaUserProfile userProfile)
    {
        ymm_reportUsertProfileJSON (JsonStringFromDictionary (userProfile.ToHashtable ()));
    }

    public override void ReportRevenue (YandexAppMetricaRevenue revenue)
    {
        ymm_reportRevenueJSON (JsonStringFromDictionary (revenue.ToHashtable ()));
    }

    public override void SetStatisticsSending (bool enabled)
    {
        ymm_setStatisticsSending (enabled);
    }

    public override void SendEventsBuffer ()
    {
        ymm_sendEventsBuffer ();
    }

    public override void RequestAppMetricaDeviceID (Action<string, YandexAppMetricaRequestDeviceIDError?> action)
    {
        ymm_requestAppMetricaDeviceID (RequestDeviceIDCallback, ActionToIntPtr (action));
    }
    
    public override void ReportAppOpen (string deeplink)
    {
        ymm_reportAppOpen (deeplink);
    }

    public override void ReportReferralUrl (string referralUrl)
    {
        ymm_reportReferralUrl (referralUrl);
    }

    #endregion

    private string JsonStringFromDictionary (IDictionary dictionary)
    {
        return dictionary == null ? null : YMMJSONUtils.JSONEncoder.Encode (dictionary);
    }

    private static IntPtr ActionToIntPtr (object obj)
    {
        if (obj == null) {
            return IntPtr.Zero;
        }

        return GCHandle.ToIntPtr (GCHandle.Alloc (obj));
    }

    private static Action<string, YandexAppMetricaRequestDeviceIDError?> IntPtrToAction (IntPtr actionPtr)
    {
        if (IntPtr.Zero.Equals (actionPtr)) {
            return null;
        }

        var gcHandle = GCHandle.FromIntPtr (actionPtr);
        return gcHandle.Target as Action<string, YandexAppMetricaRequestDeviceIDError?>;
    }

    [AOT.MonoPInvokeCallback (typeof (YMMRequestDeviceIDCallbackDelegate))]
    private static void RequestDeviceIDCallback (IntPtr actionPtr, string deviceId, string errorString)
    {
        Action<string, YandexAppMetricaRequestDeviceIDError?> action = IntPtrToAction (actionPtr);
        if (action != null) {
            action.Invoke (deviceId, RequestDeviceIDErrorFromString (errorString));
        }
    }

    private static YandexAppMetricaRequestDeviceIDError? RequestDeviceIDErrorFromString (string errorString) 
    {
        if (string.IsNullOrEmpty (errorString)) {
            return null;
        }
        try {
            var error = Enum.Parse (typeof (YandexAppMetricaRequestDeviceIDError), errorString);
            return (YandexAppMetricaRequestDeviceIDError?) error;
        } catch (ArgumentException) {
            return YandexAppMetricaRequestDeviceIDError.UNKNOWN;
        }
    }
}

public static class YandexAppMetricaExtensionsIOS
{
    public static Hashtable ToHashtable (this YandexAppMetricaConfig self)
    {
        var data = new Hashtable {
            { "ApiKey", self.ApiKey },
        };

        if (self.AppVersion != null) {
            data ["AppVersion"] = self.AppVersion;
        }
        if (self.Location.HasValue) {
            var location = self.Location.Value;
            data ["Location"] = new Hashtable {
                { "Latitude", location.Latitude },
                { "Longitude", location.Longitude },
            };
        }
        if (self.SessionTimeout.HasValue) {
            data ["SessionTimeout"] = self.SessionTimeout.Value;
        }
        if (self.CrashReporting.HasValue) {
            data ["CrashReporting"] = self.CrashReporting.Value;
        }
        if (self.LocationTracking.HasValue) {
            data ["LocationTracking"] = self.LocationTracking.Value;
        }
        if (self.Logs.HasValue) {
            data ["Logs"] = self.Logs.Value;
        }
        if (self.HandleFirstActivationAsUpdate.HasValue) {
            data ["HandleFirstActivationAsUpdate"] = self.HandleFirstActivationAsUpdate.Value;
        }

        if (self.PreloadInfo.HasValue) {
            var preloadInfo = self.PreloadInfo.Value;
            data ["PreloadInfo"] = new Hashtable {
                { "TrackingId", preloadInfo.TrackingId },
                { "AdditionalInfo", new Hashtable (preloadInfo.AdditionalInfo) },
            };
        }
        if (self.StatisticsSending.HasValue) {
            data["StatisticsSending"] = self.StatisticsSending.Value;
        }
        if (self.AppForKids.HasValue) {
            data["AppForKids"] = self.AppForKids.Value;
        }

        return data;
    }

    public static Hashtable ToHashtable (this YandexAppMetricaUserProfile self)
    {
        if (self == null) {
            return null;
        }
        var data = new Hashtable ();
        var userProfileUpdates = self.GetUserProfileUpdates ();
        for (int i = 0; i < userProfileUpdates.Count; ++i) {
            data[i.ToString ()] = new Hashtable {
                { "AttributeName", userProfileUpdates[i].AttributeName },
                { "MethodName", userProfileUpdates[i].MethodName },
                { "Key", userProfileUpdates[i].Key },
                { "Values", userProfileUpdates[i].Values }
            };
        }
        return data;
    }

    public static Hashtable ToHashtable (this YandexAppMetricaReceipt self)
    {
        var data = new Hashtable ();
        if (self.Data != null) {
            data ["Data"] = self.Data;
        }
        if (self.TransactionID != null) {
            data ["TransactionID"] = self.TransactionID;
        }
        return data;
    }

    public static Hashtable ToHashtable (this YandexAppMetricaRevenue self)
    {
        var data = new Hashtable {
            { "Currency", self.Currency }
        };
        if (self.Price.HasValue) {
            data["Price"] = self.Price.Value;
        }
        if (self.PriceDecimal.HasValue) {
            data["PriceDecimal"] = self.PriceDecimal.Value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }
        if (self.Quantity.HasValue) {
            data["Quantity"] = self.Quantity.Value;
        }
        if (self.ProductID != null) {
            data["ProductID"] = self.ProductID;
        }
        if (self.Payload != null) {
            data["Payload"] = self.Payload;
        }
        if (self.Receipt.HasValue) {
            data["Receipt"] = self.Receipt.Value.ToHashtable ();
        }
        return data;
    }
}

#endif
