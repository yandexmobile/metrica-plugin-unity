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

#if UNITY_ANDROID

public class YandexAppMetricaAndroid : BaseYandexAppMetrica
{
    
    #region IYandexAppMetrica implementation

    private readonly AndroidJavaClass metricaClass = new AndroidJavaClass ("com.yandex.metrica.YandexMetrica");

    public override void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
        base.ActivateWithConfiguration (config);
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
            metricaClass.CallStatic ("activate", playerActivityContext, config.ToAndroidAppMetricaConfig ());
        }
    }

    public override void ResumeSession ()
    {
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
            metricaClass.CallStatic ("resumeSession", playerActivityContext);
        }
    }

    public override void PauseSession ()
    {
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
            metricaClass.CallStatic ("pauseSession", playerActivityContext);
        }
    }

    public override void ReportEvent (string message)
    {
        metricaClass.CallStatic ("reportEvent", message);
    }

    public override void ReportEvent (string message, Dictionary<string, object> parameters)
    {
        metricaClass.CallStatic ("reportEvent", message, JsonStringFromDictionary (parameters));
    }

    public override void ReportError (string condition, string stackTrace)
    {
        var throwableObject = new AndroidJavaObject ("java.lang.Throwable", "\n" + stackTrace);
        metricaClass.CallStatic ("reportError", condition, throwableObject);
    }

    public override void SetLocationTracking (bool enabled)
    {
        metricaClass.CallStatic ("setLocationTracking", enabled);
    }

    public override void SetLocation (YandexAppMetricaConfig.Coordinates? coordinates)
    {
        if (coordinates.HasValue) {
            metricaClass.CallStatic ("setLocation", coordinates.Value.ToAndroidLocation ());
        } else {
            metricaClass.CallStatic ("setLocation", null);
        }
    }

    public override int LibraryApiLevel {
        get {
            return metricaClass.CallStatic<int> ("getLibraryApiLevel");
        }
    }

    public override string LibraryVersion {
        get {
            return metricaClass.CallStatic<string> ("getLibraryVersion");
        }
    }

    public override void SetUserProfileID (string userProfileID)
    {
        metricaClass.CallStatic ("setUserProfileID", userProfileID);
    }

    public override void ReportUserProfile (YandexAppMetricaUserProfile userProfile)
    {
        metricaClass.CallStatic ("reportUserProfile", userProfile.ToAndroidUserProfile ());
    }

    public override void ReportRevenue (YandexAppMetricaRevenue revenue)
    {
        metricaClass.CallStatic ("reportRevenue", revenue.ToAndroidRevenue ());
    }

    public override void SetStatisticsSending (bool enabled)
    {
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
            metricaClass.CallStatic ("setStatisticsSending", playerActivityContext, enabled);
        }
    }

    public override void SendEventsBuffer ()
    {
        metricaClass.CallStatic ("sendEventsBuffer");
    }

    public override void RequestAppMetricaDeviceID (Action<string, YandexAppMetricaRequestDeviceIDError?> action)
    {
        metricaClass.CallStatic ("requestAppMetricaDeviceID", new YandexAppMetricaDeviceIDListenerAndroid (action));
    }
    
    public override void ReportAppOpen (string deeplink)
    {
        if (string.IsNullOrEmpty(deeplink) == false) {
            metricaClass.CallStatic("reportAppOpen", deeplink);
        }
    }

    public override void ReportReferralUrl (string referralUrl)
    {
        if (string.IsNullOrEmpty(referralUrl) == false) {
            metricaClass.CallStatic("reportReferralUrl", referralUrl);
        }
    }

    #endregion

    private string JsonStringFromDictionary (IDictionary dictionary)
    {
        return dictionary == null ? null : YMMJSONUtils.JSONEncoder.Encode (dictionary);
    }

}

public static class YandexAppMetricaExtensionsAndroid
{
    public static AndroidJavaObject ToAndroidAppMetricaConfig (this YandexAppMetricaConfig self)
    {
        AndroidJavaObject appMetricaConfig = null;
        using (var configClass = new AndroidJavaClass ("com.yandex.metrica.YandexMetricaConfig")) {
            var builder = configClass.CallStatic<AndroidJavaObject> ("newConfigBuilder", self.ApiKey);

            if (self.Location.HasValue) {
                var location = self.Location.Value;
                builder.Call<AndroidJavaObject> ("withLocation", location.ToAndroidLocation ());
            }
            if (self.AppVersion != null) {
                builder.Call<AndroidJavaObject> ("withAppVersion", self.AppVersion);
            }
            if (self.LocationTracking.HasValue) {
                builder.Call<AndroidJavaObject> ("withLocationTracking", self.LocationTracking.Value);
            }
            if (self.SessionTimeout.HasValue) {
                builder.Call<AndroidJavaObject> ("withSessionTimeout", self.SessionTimeout.Value);
            }
            if (self.CrashReporting.HasValue) {
                builder.Call<AndroidJavaObject> ("withCrashReporting", self.CrashReporting.Value);
            }
            if (self.Logs ?? false) {
                builder.Call<AndroidJavaObject> ("withLogs");
            }
            if (self.InstalledAppCollecting.HasValue) {
                builder.Call<AndroidJavaObject> ("withInstalledAppCollecting", self.InstalledAppCollecting.Value);
            }
            if (self.HandleFirstActivationAsUpdate.HasValue) {
                builder.Call<AndroidJavaObject> ("handleFirstActivationAsUpdate", self.HandleFirstActivationAsUpdate.Value);
            }
            if (self.PreloadInfo.HasValue) {
                var preloadInfo = self.PreloadInfo.Value;
                var preloadInfoClass = new AndroidJavaClass ("com.yandex.metrica.PreloadInfo");
                var preloadInfoBuilder = preloadInfoClass.CallStatic<AndroidJavaObject> ("newBuilder", preloadInfo.TrackingId);
                foreach (var kvp in preloadInfo.AdditionalInfo) {
                    preloadInfoBuilder.Call<AndroidJavaObject> ("setAdditionalParams", kvp.Key, kvp.Value);
                }
                builder.Call<AndroidJavaObject> ("withPreloadInfo", preloadInfoBuilder.Call<AndroidJavaObject> ("build"));
            }
            if (self.StatisticsSending.HasValue) {
                builder.Call<AndroidJavaObject> ("withStatisticsSending", self.StatisticsSending.Value);
            }

            // Native crashes are currently not supported
            builder.Call<AndroidJavaObject> ("withNativeCrashReporting", false);
            appMetricaConfig = builder.Call<AndroidJavaObject> ("build");
        }
        return appMetricaConfig;
    }

    public static AndroidJavaObject ToAndroidLocation (this YandexAppMetricaConfig.Coordinates self)
    {
        AndroidJavaObject location = new AndroidJavaObject ("android.location.Location", "");
        location.Call ("setLatitude", self.Latitude);
        location.Call ("setLongitude", self.Longitude);
        return location;
    }

    public static AndroidJavaObject ToAndroidGender (this string self)
    {
        AndroidJavaObject gender = null;
        if (self != null) {
            using (var genderClass = new AndroidJavaClass ("com.yandex.metrica.profile.GenderAttribute$Gender")) {
                gender = genderClass.GetStatic<AndroidJavaObject> (self);
            }
        }
        return gender;
    }

    public static AndroidJavaObject ToAndroidUserProfileUpdate (this YandexAppMetricaUserProfileUpdate self)
    {
        AndroidJavaObject userProfileUpdate = null;
        AndroidJavaObject attribute = null;
        using (var attributeClass = new AndroidJavaClass ("com.yandex.metrica.profile.Attribute")) {
            if (self.Key != null) {
                attribute = attributeClass.CallStatic<AndroidJavaObject> (self.AttributeName, self.Key);
            }
            else {
                attribute = attributeClass.CallStatic<AndroidJavaObject> (self.AttributeName);
            }
        }
        if (self.AttributeName == "gender" && self.Values.Length > 0) {
            self.Values[0] = (self.Values[0] as string).ToAndroidGender ();
        }
        userProfileUpdate = attribute.Call<AndroidJavaObject> (self.MethodName, self.Values);
        return userProfileUpdate;
    }

    public static AndroidJavaObject ToAndroidUserProfile (this YandexAppMetricaUserProfile self)
    {
        AndroidJavaObject userProfile = null;
        if (self != null) {
            using (var userProfileClass = new AndroidJavaClass ("com.yandex.metrica.profile.UserProfile")) {
                var builder = userProfileClass.CallStatic<AndroidJavaObject> ("newBuilder");
                List<YandexAppMetricaUserProfileUpdate> updates = self.GetUserProfileUpdates ();
                foreach (var userProfileUpdate in updates) {
                    builder.Call<AndroidJavaObject> ("apply", userProfileUpdate.ToAndroidUserProfileUpdate ());
                }
                userProfile = builder.Call<AndroidJavaObject> ("build");
            }
        }
        return userProfile;
    }

    public static AndroidJavaObject ToAndroidReceipt (this YandexAppMetricaReceipt? self) {
        AndroidJavaObject receipt = null;
        if (self.HasValue) {
            using (var receiptClass = new AndroidJavaClass ("com.yandex.metrica.Revenue$Receipt")) {
                var builder = receiptClass.CallStatic<AndroidJavaObject> ("newBuilder");
                builder.Call<AndroidJavaObject> ("withData", self.Value.Data);
                builder.Call<AndroidJavaObject> ("withSignature", self.Value.Signature);
                receipt = builder.Call<AndroidJavaObject> ("build");
            }
        }
        return receipt;
    }

    public static AndroidJavaObject ToAndroidInteger (this int? self) {
        AndroidJavaObject integer = null;
        if (self.HasValue) {
            using (var integerClass = new AndroidJavaClass ("java.lang.Integer")) {
                integer = integerClass.CallStatic<AndroidJavaObject> ("valueOf", self);
            }
        }
        return integer;
    }

    public static AndroidJavaObject ToAndroidCurrency (this string self) {
        AndroidJavaObject currency = null;
        if (self != null) {
            using (var currencyClass = new AndroidJavaClass ("java.util.Currency")) {
                currency = currencyClass.CallStatic<AndroidJavaObject> ("getInstance", self);
            }
        }
        return currency;
    }

    public static AndroidJavaObject ToAndroidRevenue (this YandexAppMetricaRevenue self)
    {
        AndroidJavaObject revenue = null;
        using (var revenueClass = new AndroidJavaClass ("com.yandex.metrica.Revenue")) {
            AndroidJavaObject builder;
            if (self.PriceDecimal.HasValue) {
                var priceMicros = decimal.ToInt64(self.PriceDecimal.Value * 1000000m);
                builder = revenueClass.CallStatic<AndroidJavaObject> ("newBuilderWithMicros", priceMicros, self.Currency.ToAndroidCurrency ());
            } else {
                builder = revenueClass.CallStatic<AndroidJavaObject> ("newBuilder", self.Price, self.Currency.ToAndroidCurrency ());
            }
            builder.Call<AndroidJavaObject> ("withQuantity", self.Quantity.ToAndroidInteger ());
            builder.Call<AndroidJavaObject> ("withProductID", self.ProductID);
            builder.Call<AndroidJavaObject> ("withPayload", self.Payload);
            builder.Call<AndroidJavaObject> ("withReceipt", self.Receipt.ToAndroidReceipt ());
            revenue = builder.Call<AndroidJavaObject> ("build");
        }
        return revenue;
    }
}

public class YandexAppMetricaDeviceIDListenerAndroid : AndroidJavaProxy
{
    private readonly Action<string, YandexAppMetricaRequestDeviceIDError?> action;

    public YandexAppMetricaDeviceIDListenerAndroid (Action<string, YandexAppMetricaRequestDeviceIDError?> action) 
        : base ("com.yandex.metrica.AppMetricaDeviceIDListener")
    {
        this.action = action;
    }

    public void onLoaded(string deviceID) {
        action.Invoke (deviceID, null);
    }

    public void onError(AndroidJavaObject reason) {
        action.Invoke (null, ErrorFromAndroidReason (reason));
    }

    private YandexAppMetricaRequestDeviceIDError? ErrorFromAndroidReason (AndroidJavaObject reason) {
        if (reason == null) {
            return null;
        }
        try {
            var reasonString = reason.Call<string> ("toString");
            var error = Enum.Parse (typeof (YandexAppMetricaRequestDeviceIDError), reasonString);
            return (YandexAppMetricaRequestDeviceIDError?) error;
        } catch (ArgumentException) {
            return YandexAppMetricaRequestDeviceIDError.UNKNOWN;
        }
    }
}

#endif
