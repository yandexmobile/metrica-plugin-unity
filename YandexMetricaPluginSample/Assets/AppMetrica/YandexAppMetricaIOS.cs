/*
 * Version for Unity
 * © 2015-2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

    #endregion

    private string JsonStringFromDictionary (IDictionary dictionary)
    {
        return dictionary == null ? null : YMMJSONUtils.JSONEncoder.Encode (dictionary);
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
        Hashtable data = new Hashtable {
            { "Data", self.Data },
            { "TransactionID", self.TransactionID }
        };
        return data;
    }

    public static Hashtable ToHashtable (this YandexAppMetricaRevenue self)
    {
        var data = new Hashtable {
            { "Price", self.Price },
            { "Currency", self.Currency }
        };
        if (self.Quantity.HasValue) {
            data ["Quantity"] = self.Quantity.Value;
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
