/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if UNITY_IPHONE || UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using YMMJSONUtils;

public class YandexAppMetricaIOS : BaseYandexAppMetrica
{
    [DllImport("__Internal")]
    private static extern void ymm_activateWithConfigurationJSON(string configurationJson);

    [DllImport("__Internal")]
    private static extern void ymm_resumeSession();

    [DllImport("__Internal")]
    private static extern void ymm_pauseSession();

    [DllImport("__Internal")]
    private static extern void ymm_reportAdRevenueJSON(string adRevenueJson);

    [DllImport("__Internal")]
    private static extern void ymm_reportEvent(string message);

    [DllImport("__Internal")]
    private static extern void ymm_reportEventWithParameters(string message, string parameters);

    [DllImport("__Internal")]
    private static extern void ymm_reportError(string condition, string stackTrace);

    [DllImport("__Internal")]
    private static extern void ymm_reportErrorWithIdentifier(
        string groupIdentifier, string condition, string stackTrace);

    [DllImport("__Internal")]
    private static extern void ymm_reportErrorWithException(
        string groupIdentifier, string condition, string exceptionJson);

    [DllImport("__Internal")]
    private static extern void ymm_reportUnhandledException(string errorJson);

    [DllImport("__Internal")]
    private static extern void ymm_reportErrorWithMessage(string errorJson, string message);

    [DllImport("__Internal")]
    private static extern void ymm_reportErrorWithIdentifierAndMessage(
        string groupIdentifier, string message, string errorJson);

    [DllImport("__Internal")]
    private static extern void ymm_setLocationTracking(bool enabled);

    [DllImport("__Internal")]
    private static extern void ymm_setLocation(double latitude, double longitude);

    [DllImport("__Internal")]
    private static extern void ymm_resetLocation();

    [DllImport("__Internal")]
    private static extern string ymm_getLibraryVersion();

    [DllImport("__Internal")]
    private static extern void ymm_setUserProfileID(string userProfileID);

    [DllImport("__Internal")]
    private static extern void ymm_reportUsertProfileJSON(string userProfileJson);

    [DllImport("__Internal")]
    private static extern void ymm_reportRevenueJSON(string revenueJson);

    [DllImport("__Internal")]
    private static extern void ymm_setStatisticsSending(bool enabled);

    [DllImport("__Internal")]
    private static extern void ymm_sendEventsBuffer();

    [DllImport("__Internal")]
    private static extern void ymm_requestAppMetricaDeviceID(
        YMMRequestDeviceIDCallbackDelegate callbackDelegate, IntPtr actionPtr);

    [DllImport("__Internal")]
    private static extern void ymm_reportReferralUrl(string referralUrl);

    [DllImport("__Internal")]
    private static extern void ymm_reportAppOpen(string deeplink);

    [DllImport("__Internal")]
    private static extern void ymm_putErrorEnvironmentValue(string key, string value);

    [DllImport("__Internal")]
    private static extern void ymm_requestTrackingAuthorization(
        YMMRequestTrackingAuthorization callbackDelegate, IntPtr actionPtr);

    private static string JsonStringFromDictionary(IEnumerable dictionary)
    {
        return dictionary == null ? null : JSONEncoder.Encode(dictionary);
    }

    private static IntPtr ActionToIntPtr(object obj)
    {
        return obj == null ? IntPtr.Zero : GCHandle.ToIntPtr(GCHandle.Alloc(obj));
    }

    private static Action<string, YandexAppMetricaRequestDeviceIDError?> ActionForRequestAppMetricaDeviceID(
        IntPtr actionPtr)
    {
        if (IntPtr.Zero.Equals(actionPtr))
        {
            return null;
        }

        GCHandle gcHandle = GCHandle.FromIntPtr(actionPtr);
        return gcHandle.Target as Action<string, YandexAppMetricaRequestDeviceIDError?>;
    }

    [MonoPInvokeCallback(typeof(YMMRequestDeviceIDCallbackDelegate))]
    private static void RequestDeviceIDCallback(IntPtr actionPtr, string deviceId, string errorString)
    {
        Action<string, YandexAppMetricaRequestDeviceIDError?> action = ActionForRequestAppMetricaDeviceID(actionPtr);
        if (action != null)
        {
            action.Invoke(deviceId, RequestDeviceIDErrorFromString(errorString));
        }
    }

    private static YandexAppMetricaRequestDeviceIDError? RequestDeviceIDErrorFromString(string errorString)
    {
        if (string.IsNullOrEmpty(errorString))
        {
            return null;
        }

        try
        {
            object error = Enum.Parse(typeof(YandexAppMetricaRequestDeviceIDError), errorString);
            return (YandexAppMetricaRequestDeviceIDError?)error;
        }
        catch (ArgumentException)
        {
            return YandexAppMetricaRequestDeviceIDError.UNKNOWN;
        }
    }

    private static Action<YandexAppMetricaRequestTrackingStatus> ActionForRequestTrackingAuthorization(IntPtr actionPtr)
    {
        if (IntPtr.Zero.Equals(actionPtr))
        {
            return null;
        }

        GCHandle gcHandle = GCHandle.FromIntPtr(actionPtr);
        return gcHandle.Target as Action<YandexAppMetricaRequestTrackingStatus>;
    }

    [MonoPInvokeCallback(typeof(YMMRequestTrackingAuthorization))]
    private static void RequestTrackingAuthorizationCallback(IntPtr actionPtr, long status)
    {
        Action<YandexAppMetricaRequestTrackingStatus> action = ActionForRequestTrackingAuthorization(actionPtr);
        if (action != null)
        {
            action.Invoke((YandexAppMetricaRequestTrackingStatus)status);
        }
    }

    private delegate void YMMRequestDeviceIDCallbackDelegate(IntPtr actionPtr, string deviceId, string errorString);

    private delegate void YMMRequestTrackingAuthorization(IntPtr actionPtr, long status);

    #region IYandexAppMetrica implementation

    public override void ActivateWithConfiguration(YandexAppMetricaConfig config)
    {
        base.ActivateWithConfiguration(config);
        ymm_activateWithConfigurationJSON(JsonStringFromDictionary(config.ToHashtable()));
    }

    public override void ResumeSession()
    {
        ymm_resumeSession();
    }

    public override void PauseSession()
    {
        ymm_pauseSession();
    }

    public override void ReportAdRevenue(YandexAppMetricaAdRevenue adRevenue)
    {
        ymm_reportAdRevenueJSON(JsonStringFromDictionary(adRevenue.ToHashtable()));
    }

    public override void ReportEvent(string message)
    {
        ymm_reportEvent(message);
    }

    public override void ReportEvent(string message, IDictionary<string, object> parameters)
    {
        ymm_reportEventWithParameters(message, JsonStringFromDictionary(parameters));
    }

    public override void ReportEvent(string message, string json)
    {
        ymm_reportEventWithParameters(message, json);
    }

    [Obsolete("Use the ReportError(Exception exception, string condition) instead.")]
    public override void ReportError(string condition, string stackTrace)
    {
        ymm_reportError(condition, stackTrace);
    }

    [Obsolete("Use the ReportError(string groupIdentifier, string condition, Exception exception) instead.")]
    public override void ReportError(string groupIdentifier, string condition, string stackTrace)
    {
        ymm_reportErrorWithIdentifier(groupIdentifier, condition, stackTrace);
    }

    public override void ReportError(string groupIdentifier, string condition, Exception exception)
    {
        try
        {
            ReportError(groupIdentifier, condition,
                exception == null ? null : YandexAppMetricaErrorDetails.From(exception));
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.Log("[AppMetrica] Failed to parse stacktrace: " + e.Message + "\n" + e.StackTrace);
#endif
            // use old crash format
            ymm_reportErrorWithException(groupIdentifier, condition, JsonStringFromDictionary(exception.ToHashtable()));
        }
    }

    public override void ReportError(
        string groupIdentifier,
        string condition,
        YandexAppMetricaErrorDetails errorDetails)
    {
        ymm_reportErrorWithIdentifierAndMessage(groupIdentifier, condition,
            JsonStringFromDictionary(errorDetails.ToHashtable()));
    }

    public override void ReportError(Exception exception, string condition)
    {
        try
        {
            ReportError(YandexAppMetricaErrorDetails.From(exception), condition);
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.Log("[AppMetrica] Failed to parse stacktrace: " + e.Message + "\n" + e.StackTrace);
#endif
            // use old crash format
            ymm_reportError(condition, exception.StackTrace);
        }
    }

    public override void ReportError(YandexAppMetricaErrorDetails errorDetails, string condition)
    {
        ymm_reportErrorWithMessage(JsonStringFromDictionary(errorDetails.ToHashtable()), condition);
    }

    public override void ReportUnhandledException(Exception exception)
    {
        try
        {
            ReportUnhandledException(YandexAppMetricaErrorDetails.From(exception));
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.Log("[AppMetrica] Failed to parse stacktrace: " + e.Message + "\n" + e.StackTrace);
#endif
            // use old crash format
            ymm_reportError(exception.Message, exception.StackTrace);
        }
    }

    public override void ReportUnhandledException(YandexAppMetricaErrorDetails errorDetails)
    {
        ymm_reportUnhandledException(JsonStringFromDictionary(errorDetails.ToHashtable()));
    }

    public override void ReportErrorFromLogCallback(string condition, string stackTrace)
    {
        try
        {
            ReportError(YandexAppMetricaErrorDetails.FromLogCallback(condition, stackTrace), condition);
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.Log("[AppMetrica] Failed to parse stacktrace: " + e.Message + "\n" + e.StackTrace);
#endif
            // use old crash format
            ymm_reportErrorWithIdentifier(condition, condition, stackTrace);
        }
    }

    public override void SetLocationTracking(bool enabled)
    {
        ymm_setLocationTracking(enabled);
    }

    public override void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates)
    {
        if (coordinates.HasValue)
        {
            ymm_setLocation(coordinates.Value.Latitude, coordinates.Value.Longitude);
        }
        else
        {
            ymm_resetLocation();
        }
    }

    // Not available for iOS
    public override int LibraryApiLevel
    {
        get
        {
            return 0;
        }
    }

    public override string LibraryVersion
    {
        get
        {
            return ymm_getLibraryVersion();
        }
    }

    public override void SetUserProfileID(string userProfileID)
    {
        ymm_setUserProfileID(userProfileID);
    }

    public override void ReportUserProfile(YandexAppMetricaUserProfile userProfile)
    {
        ymm_reportUsertProfileJSON(JsonStringFromDictionary(userProfile.ToHashtable()));
    }

    public override void ReportRevenue(YandexAppMetricaRevenue revenue)
    {
        ymm_reportRevenueJSON(JsonStringFromDictionary(revenue.ToHashtable()));
    }

    public override void SetStatisticsSending(bool enabled)
    {
        ymm_setStatisticsSending(enabled);
    }

    public override void SendEventsBuffer()
    {
        ymm_sendEventsBuffer();
    }

    public override void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action)
    {
        ymm_requestAppMetricaDeviceID(RequestDeviceIDCallback, ActionToIntPtr(action));
    }

    public override void ReportAppOpen(string deeplink)
    {
        ymm_reportAppOpen(deeplink);
    }

    public override void PutErrorEnvironmentValue(string key, string value)
    {
        ymm_putErrorEnvironmentValue(key, value);
    }

    public override void ReportReferralUrl(string referralUrl)
    {
        ymm_reportReferralUrl(referralUrl);
    }

    public override void RequestTrackingAuthorization(Action<YandexAppMetricaRequestTrackingStatus> action)
    {
        ymm_requestTrackingAuthorization(RequestTrackingAuthorizationCallback, ActionToIntPtr(action));
    }

    #endregion
}

public static class YandexAppMetricaExtensionsIOS
{
    public static Hashtable ToHashtable(this YandexAppMetricaConfig self)
    {
        Hashtable data = new Hashtable { { "ApiKey", self.ApiKey } };

        if (self.AppVersion != null)
        {
            data["AppVersion"] = self.AppVersion;
        }

        if (self.Location.HasValue)
        {
            YandexAppMetricaConfig.Coordinates location = self.Location.Value;
            data["Location"] = new Hashtable { { "Latitude", location.Latitude }, { "Longitude", location.Longitude } };
        }

        if (self.SessionTimeout.HasValue)
        {
            data["SessionTimeout"] = self.SessionTimeout.Value;
        }

        if (self.CrashReporting.HasValue)
        {
            data["CrashReporting"] = self.CrashReporting.Value;
        }

        if (self.LocationTracking.HasValue)
        {
            data["LocationTracking"] = self.LocationTracking.Value;
        }

        if (self.Logs.HasValue)
        {
            data["Logs"] = self.Logs.Value;
        }

        if (self.HandleFirstActivationAsUpdate.HasValue)
        {
            data["HandleFirstActivationAsUpdate"] = self.HandleFirstActivationAsUpdate.Value;
        }

        if (self.PreloadInfo.HasValue)
        {
            YandexAppMetricaPreloadInfo preloadInfo = self.PreloadInfo.Value;
            data["PreloadInfo"] = new Hashtable
            {
                { "TrackingId", preloadInfo.TrackingId },
                { "AdditionalInfo", new Hashtable(preloadInfo.AdditionalInfo) }
            };
        }

        if (self.StatisticsSending.HasValue)
        {
            data["StatisticsSending"] = self.StatisticsSending.Value;
        }

        if (self.AppForKids.HasValue)
        {
            data["AppForKids"] = self.AppForKids.Value;
        }

        if (self.UserProfileID != null)
        {
            data["UserProfileID"] = self.UserProfileID;
        }

        if (self.RevenueAutoTrackingEnabled.HasValue)
        {
            data["RevenueAutoTrackingEnabled"] = self.RevenueAutoTrackingEnabled.Value;
        }

        return data;
    }

    public static Hashtable ToHashtable(this YandexAppMetricaUserProfile self)
    {
        if (self == null)
        {
            return null;
        }

        Hashtable data = new Hashtable();
        List<YandexAppMetricaUserProfileUpdate> userProfileUpdates = self.GetUserProfileUpdates();
        for (int i = 0; i < userProfileUpdates.Count; ++i)
        {
            data[i.ToString()] = new Hashtable
            {
                { "AttributeName", userProfileUpdates[i].AttributeName },
                { "MethodName", userProfileUpdates[i].MethodName },
                { "Key", userProfileUpdates[i].Key },
                { "Values", userProfileUpdates[i].Values }
            };
        }

        return data;
    }

    public static Hashtable ToHashtable(this YandexAppMetricaReceipt self)
    {
        Hashtable data = new Hashtable();
        if (self.Data != null)
        {
            data["Data"] = self.Data;
        }

        if (self.TransactionID != null)
        {
            data["TransactionID"] = self.TransactionID;
        }

        return data;
    }

    public static Hashtable ToHashtable(this YandexAppMetricaRevenue self)
    {
        Hashtable data = new Hashtable { { "Currency", self.Currency } };
        if (self.Price.HasValue)
        {
            data["Price"] = self.Price.Value;
        }

        if (self.PriceDecimal.HasValue)
        {
            data["PriceDecimal"] = self.PriceDecimal.Value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }

        if (self.Quantity.HasValue)
        {
            data["Quantity"] = self.Quantity.Value;
        }

        if (self.ProductID != null)
        {
            data["ProductID"] = self.ProductID;
        }

        if (self.Payload != null)
        {
            data["Payload"] = self.Payload;
        }

        if (self.Receipt.HasValue)
        {
            data["Receipt"] = self.Receipt.Value.ToHashtable();
        }

        return data;
    }

    public static Hashtable ToHashtable(this YandexAppMetricaAdRevenue self)
    {
        Hashtable data = new Hashtable
        {
            { "AdRevenue", self.AdRevenue.ToString(CultureInfo.CreateSpecificCulture("en-US")) },
            { "Currency", self.Currency }
        };
        if (self.AdType.HasValue)
        {
            data["AdType"] = self.AdType.Value.ToString();
        }
        if (self.AdNetwork != null)
        {
            data["AdNetwork"] = self.AdNetwork;
        }
        if (self.AdUnitId != null)
        {
            data["AdUnitId"] = self.AdUnitId;
        }
        if (self.AdUnitName != null)
        {
            data["AdUnitName"] = self.AdUnitName;
        }
        if (self.AdPlacementId != null)
        {
            data["AdPlacementId"] = self.AdPlacementId;
        }
        if (self.AdPlacementName != null)
        {
            data["AdPlacementName"] = self.AdPlacementName;
        }
        if (self.Precision != null)
        {
            data["Precision"] = self.Precision;
        }
        if (self.Payload != null)
        {
            data["Payload"] = JSONEncoder.Encode(self.Payload);
        }

        return data;
    }

    public static Hashtable ToHashtable(this Exception self)
    {
        if (self == null)
        {
            return null;
        }

        return new Hashtable
        {
            { "type", self.GetType().Name }, { "message", self.Message }, { "stacktrace", self.StackTrace }
        };
    }

    public static Hashtable ToHashtable(this YandexAppMetricaErrorDetails self)
    {
        if (self == null)
        {
            return null;
        }

        Hashtable data = new Hashtable
        {
            { "Platform", self.Platform }, { "VirtualMachineVersion", self.VirtualMachineVersion }
        };
        if (self.ExceptionClass != null)
        {
            data["ExceptionClass"] = self.ExceptionClass;
        }

        if (self.Message != null)
        {
            data["Message"] = self.Message;
        }

        if (self.Stacktrace != null)
        {
            Hashtable stacktrace = new Hashtable();
            for (int i = 0; i < self.Stacktrace.Count; ++i)
            {
                stacktrace[i.ToString()] = self.Stacktrace[i].ToHashtable();
            }

            data["Stacktrace"] = stacktrace;
        }

        if (self.PluginEnvironment != null)
        {
            data["PluginEnvironment"] = new Hashtable(self.PluginEnvironment);
        }

        return data;
    }

    private static Hashtable ToHashtable(this YandexAppMetricaStackTraceItem self)
    {
        if (self == null)
        {
            return null;
        }

        Hashtable data = new Hashtable();
        if (self.ClassName != null)
        {
            data["ClassName"] = self.ClassName;
        }

        if (self.MethodName != null)
        {
            data["MethodName"] = self.MethodName;
        }

        if (self.FileName != null)
        {
            data["FileName"] = self.FileName;
        }

        if (self.Line.HasValue)
        {
            data["Line"] = self.Line.Value;
        }

        if (self.Column.HasValue)
        {
            data["Column"] = self.Column.Value;
        }

        return data;
    }
}

#endif
