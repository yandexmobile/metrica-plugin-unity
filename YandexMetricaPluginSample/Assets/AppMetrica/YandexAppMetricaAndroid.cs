/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using YMMJSONUtils;

public class YandexAppMetricaAndroid : BaseYandexAppMetrica
{
    private readonly Dictionary<string, string> _javaByteCodeClassByName = new Dictionary<string, string>
    {
        { "int", "I" },
        { "boolean", "Z" },
        { "byte", "B" },
        { "short", "S" },
        { "long", "J" },
        { "float", "F" },
        { "double", "D" },
        { "char", "C" },
        { "String", "Ljava/lang/String;" },
        { "Throwable", "Ljava/lang/Throwable;" },
        { "PluginErrorDetails", "Lcom/yandex/metrica/plugins/PluginErrorDetails;" }
    };

    private readonly AndroidJavaClass _metricaClass = new AndroidJavaClass("com.yandex.metrica.YandexMetrica");

    private static string JsonStringFromDictionary(IEnumerable dictionary)
    {
        return dictionary == null ? null : JSONEncoder.Encode(dictionary);
    }

    private static AndroidJavaObject ThrowableFromStringStackTrace(string stackTrace)
    {
        return new AndroidJavaObject("java.lang.Throwable", "\n" + stackTrace);
    }

    private void CallAppMetrica(string methodName, IEnumerable<string> types, params object[] args)
    {
        IntPtr methodID = AndroidJNIHelper.GetMethodID(_metricaClass.GetRawClass(), methodName,
            GetSignatureFromTypeNames(types), true);
        jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
        try
        {
            AndroidJNI.CallStaticVoidMethod(_metricaClass.GetRawClass(), methodID, jniArgArray);
        }
        finally
        {
            AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
        }
    }

    private void CallYandexMetricaPlugins(
        [NotNull] string methodName,
        [NotNull] IEnumerable<string> types,
        [NotNull] params object[] args)
    {
        using (AndroidJavaObject pluginsImpl = _metricaClass.CallStatic<AndroidJavaObject>("getPluginExtension"))
        {
            IntPtr methodID = AndroidJNIHelper.GetMethodID(pluginsImpl.GetRawClass(), methodName,
                GetSignatureFromTypeNames(types), false);
            jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
            try
            {
                AndroidJNI.CallVoidMethod(pluginsImpl.GetRawObject(), methodID, jniArgArray);
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
            }
        }
    }

    private string GetSignatureFromTypeNames(IEnumerable<string> types)
    {
        StringBuilder str = new StringBuilder("(");
        foreach (string type in types)
        {
            if (_javaByteCodeClassByName.ContainsKey(type))
            {
                str.Append(_javaByteCodeClassByName[type]);
            }
            else
            {
                str.AppendFormat("L{0};", type.Replace('.', '/'));
            }
        }

        str.Append(")V");
        return str.ToString();
    }

    #region IYandexAppMetrica implementation

    public override void ActivateWithConfiguration(YandexAppMetricaConfig config)
    {
        base.ActivateWithConfiguration(config);
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject androidConfig = config.ToAndroidAppMetricaConfig();
            _metricaClass.CallStatic("activate", playerActivityContext, androidConfig);
        }
    }

    public override void ResumeSession()
    {
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _metricaClass.CallStatic("resumeSession", playerActivityContext);
        }
    }

    public override void PauseSession()
    {
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _metricaClass.CallStatic("pauseSession", playerActivityContext);
        }
    }

    public override void ReportAdRevenue(YandexAppMetricaAdRevenue adRevenue)
    {
        CallAppMetrica("reportAdRevenue", new []{"com.yandex.metrica.AdRevenue"}, adRevenue.ToAndroidAdRevenue());
    }

    public override void ReportEvent(string message)
    {
        _metricaClass.CallStatic("reportEvent", message);
    }

    public override void ReportEvent(string message, IDictionary<string, object> parameters)
    {
        _metricaClass.CallStatic("reportEvent", message, JsonStringFromDictionary(parameters));
    }

    public override void ReportEvent(string message, string json)
    {
        CallAppMetrica("reportEvent", new[] { "String", "String" }, message, json);
    }

    [Obsolete("Use the ReportError(Exception exception, string condition) instead.")]
    public override void ReportError(string condition, string stackTrace)
    {
        AndroidJavaObject throwableObject = stackTrace == null ? null : ThrowableFromStringStackTrace(stackTrace);
        CallAppMetrica("reportError", new[] { "String", "Throwable" },
            condition, throwableObject);
    }

    [Obsolete("Use the ReportError(string groupIdentifier, string condition, Exception exception) instead.")]
    public override void ReportError(string groupIdentifier, string condition, string stackTrace)
    {
        AndroidJavaObject throwableObject = stackTrace == null ? null : ThrowableFromStringStackTrace(stackTrace);
        CallAppMetrica("reportError", new[] { "String", "String", "Throwable" },
            groupIdentifier, condition, throwableObject);
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
            CallAppMetrica("reportError", new[] { "String", "String", "Throwable" },
                groupIdentifier, condition, exception.ToAndroidThrowable());
        }
    }

    public override void ReportError(
        string groupIdentifier,
        string condition,
        YandexAppMetricaErrorDetails errorDetails)
    {
        CallYandexMetricaPlugins("reportError", new[] { "String", "String", "PluginErrorDetails" },
            groupIdentifier, condition, errorDetails.ToAndroidPluginErrorDetails());
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
            CallAppMetrica("reportError", new[] { "String", "Throwable" },
                condition, exception.ToAndroidThrowable());
        }
    }

    public override void ReportError(YandexAppMetricaErrorDetails errorDetails, string condition)
    {
        CallYandexMetricaPlugins("reportError", new[] { "PluginErrorDetails", "String" },
            errorDetails.ToAndroidPluginErrorDetails(), condition);
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
            CallAppMetrica("reportUnhandledException", new[] { "Throwable" },
                exception.ToAndroidThrowable());
        }
    }

    public override void ReportUnhandledException(YandexAppMetricaErrorDetails errorDetails)
    {
        CallYandexMetricaPlugins("reportUnhandledException", new[] { "PluginErrorDetails" },
            errorDetails.ToAndroidPluginErrorDetails());
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
            AndroidJavaObject throwableObject = stackTrace == null ? null : ThrowableFromStringStackTrace(stackTrace);
            CallAppMetrica("reportError", new[] { "String", "String", "Throwable" },
                condition, condition, throwableObject);
        }
    }

    public override void SetLocationTracking(bool enabled)
    {
        _metricaClass.CallStatic("setLocationTracking", enabled);
    }

    public override void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates)
    {
        _metricaClass.CallStatic("setLocation", coordinates.HasValue ? coordinates.Value.ToAndroidLocation() : null);
    }

    public override int LibraryApiLevel
    {
        get
        {
            return _metricaClass.CallStatic<int>("getLibraryApiLevel");
        }
    }

    public override string LibraryVersion
    {
        get
        {
            return _metricaClass.CallStatic<string>("getLibraryVersion");
        }
    }

    public override void SetUserProfileID(string userProfileID)
    {
        _metricaClass.CallStatic("setUserProfileID", userProfileID);
    }

    public override void ReportUserProfile(YandexAppMetricaUserProfile userProfile)
    {
        _metricaClass.CallStatic("reportUserProfile", userProfile.ToAndroidUserProfile());
    }

    public override void ReportRevenue(YandexAppMetricaRevenue revenue)
    {
        _metricaClass.CallStatic("reportRevenue", revenue.ToAndroidRevenue());
    }

    public override void SetStatisticsSending(bool enabled)
    {
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _metricaClass.CallStatic("setStatisticsSending", playerActivityContext, enabled);
        }
    }

    public override void SendEventsBuffer()
    {
        _metricaClass.CallStatic("sendEventsBuffer");
    }

    public override void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action)
    {
        _metricaClass.CallStatic("requestAppMetricaDeviceID", new YandexAppMetricaDeviceIDListenerAndroid(action));
    }

    public override void ReportAppOpen(string deeplink)
    {
        if (string.IsNullOrEmpty(deeplink) == false)
        {
            _metricaClass.CallStatic("reportAppOpen", deeplink);
        }
    }

    public override void PutErrorEnvironmentValue(string key, string value)
    {
        _metricaClass.CallStatic("putErrorEnvironmentValue", key, value);
    }

    public override void ReportReferralUrl(string referralUrl)
    {
        if (string.IsNullOrEmpty(referralUrl) == false)
        {
            _metricaClass.CallStatic("reportReferralUrl", referralUrl);
        }
    }

    public override void RequestTrackingAuthorization(Action<YandexAppMetricaRequestTrackingStatus> action)
    {
        // Not available for Android
    }

    #endregion
}

public static class YandexAppMetricaExtensionsAndroid
{
    public static AndroidJavaObject ToAndroidAppMetricaConfig(this YandexAppMetricaConfig self)
    {
        AndroidJavaObject appMetricaConfig;
        using (AndroidJavaClass configClass = new AndroidJavaClass("com.yandex.metrica.YandexMetricaConfig"))
        {
            AndroidJavaObject builder = configClass.CallStatic<AndroidJavaObject>("newConfigBuilder", self.ApiKey);

            if (self.Location.HasValue)
            {
                YandexAppMetricaConfig.Coordinates location = self.Location.Value;
                builder.Call<AndroidJavaObject>("withLocation", location.ToAndroidLocation());
            }

            if (self.AppVersion != null)
            {
                builder.Call<AndroidJavaObject>("withAppVersion", self.AppVersion);
            }

            if (self.LocationTracking.HasValue)
            {
                builder.Call<AndroidJavaObject>("withLocationTracking", self.LocationTracking.Value);
            }

            if (self.SessionTimeout.HasValue)
            {
                builder.Call<AndroidJavaObject>("withSessionTimeout", self.SessionTimeout.Value);
            }

            if (self.CrashReporting.HasValue)
            {
                builder.Call<AndroidJavaObject>("withCrashReporting", self.CrashReporting.Value);
            }

            if (self.Logs ?? false)
            {
                builder.Call<AndroidJavaObject>("withLogs");
            }

            if (self.HandleFirstActivationAsUpdate.HasValue)
            {
                builder.Call<AndroidJavaObject>("handleFirstActivationAsUpdate",
                    self.HandleFirstActivationAsUpdate.Value);
            }

            if (self.PreloadInfo.HasValue)
            {
                YandexAppMetricaPreloadInfo preloadInfo = self.PreloadInfo.Value;
                AndroidJavaClass preloadInfoClass = new AndroidJavaClass("com.yandex.metrica.PreloadInfo");
                AndroidJavaObject preloadInfoBuilder =
                    preloadInfoClass.CallStatic<AndroidJavaObject>("newBuilder", preloadInfo.TrackingId);
                foreach (KeyValuePair<string, string> kvp in preloadInfo.AdditionalInfo)
                {
                    preloadInfoBuilder.Call<AndroidJavaObject>("setAdditionalParams", kvp.Key, kvp.Value);
                }

                builder.Call<AndroidJavaObject>("withPreloadInfo", preloadInfoBuilder.Call<AndroidJavaObject>("build"));
            }

            if (self.StatisticsSending.HasValue)
            {
                builder.Call<AndroidJavaObject>("withStatisticsSending", self.StatisticsSending.Value);
            }

            if (self.UserProfileID != null)
            {
                builder.Call<AndroidJavaObject>("withUserProfileID", self.UserProfileID);
            }

            if (self.RevenueAutoTrackingEnabled.HasValue)
            {
                builder.Call<AndroidJavaObject>("withRevenueAutoTrackingEnabled",
                    self.RevenueAutoTrackingEnabled.Value);
            }

            // Native crashes are currently not supported
            builder.Call<AndroidJavaObject>("withNativeCrashReporting", false);
            // Sessions are monitored by plugin itself
            builder.Call<AndroidJavaObject>("withSessionsAutoTrackingEnabled", false);
            appMetricaConfig = builder.Call<AndroidJavaObject>("build");
        }

        return appMetricaConfig;
    }

    public static AndroidJavaObject ToAndroidLocation(this YandexAppMetricaConfig.Coordinates self)
    {
        AndroidJavaObject location = new AndroidJavaObject("android.location.Location", "");
        location.Call("setLatitude", self.Latitude);
        location.Call("setLongitude", self.Longitude);
        return location;
    }

    public static AndroidJavaObject ToAndroidGender(this string self)
    {
        AndroidJavaObject gender = null;
        if (self != null)
        {
            using (AndroidJavaClass genderClass =
                   new AndroidJavaClass("com.yandex.metrica.profile.GenderAttribute$Gender"))
            {
                gender = genderClass.GetStatic<AndroidJavaObject>(self);
            }
        }

        return gender;
    }

    public static AndroidJavaObject ToAndroidUserProfileUpdate(this YandexAppMetricaUserProfileUpdate self)
    {
        AndroidJavaObject userProfileUpdate;
        AndroidJavaObject attribute;
        using (AndroidJavaClass attributeClass = new AndroidJavaClass("com.yandex.metrica.profile.Attribute"))
        {
            if (self.Key != null)
            {
                attribute = attributeClass.CallStatic<AndroidJavaObject>(self.AttributeName, self.Key);
            }
            else
            {
                attribute = attributeClass.CallStatic<AndroidJavaObject>(self.AttributeName);
            }
        }

        if (self.AttributeName == "gender" && self.Values.Length > 0)
        {
            self.Values[0] = (self.Values[0] as string).ToAndroidGender();
        }

        userProfileUpdate = attribute.Call<AndroidJavaObject>(self.MethodName, self.Values);
        return userProfileUpdate;
    }

    public static AndroidJavaObject ToAndroidUserProfile(this YandexAppMetricaUserProfile self)
    {
        AndroidJavaObject userProfile = null;
        if (self != null)
        {
            using (AndroidJavaClass userProfileClass = new AndroidJavaClass("com.yandex.metrica.profile.UserProfile"))
            {
                AndroidJavaObject builder = userProfileClass.CallStatic<AndroidJavaObject>("newBuilder");
                List<YandexAppMetricaUserProfileUpdate> updates = self.GetUserProfileUpdates();
                foreach (YandexAppMetricaUserProfileUpdate userProfileUpdate in updates)
                {
                    builder.Call<AndroidJavaObject>("apply", userProfileUpdate.ToAndroidUserProfileUpdate());
                }

                userProfile = builder.Call<AndroidJavaObject>("build");
            }
        }

        return userProfile;
    }

    public static AndroidJavaObject ToAndroidReceipt(this YandexAppMetricaReceipt? self)
    {
        AndroidJavaObject receipt = null;
        if (self.HasValue)
        {
            using (AndroidJavaClass receiptClass = new AndroidJavaClass("com.yandex.metrica.Revenue$Receipt"))
            {
                AndroidJavaObject builder = receiptClass.CallStatic<AndroidJavaObject>("newBuilder");
                builder.Call<AndroidJavaObject>("withData", self.Value.Data);
                builder.Call<AndroidJavaObject>("withSignature", self.Value.Signature);
                receipt = builder.Call<AndroidJavaObject>("build");
            }
        }

        return receipt;
    }

    public static AndroidJavaObject ToAndroidInteger(this int? self)
    {
        AndroidJavaObject integer = null;
        if (self.HasValue)
        {
            using (AndroidJavaClass integerClass = new AndroidJavaClass("java.lang.Integer"))
            {
                integer = integerClass.CallStatic<AndroidJavaObject>("valueOf", self);
            }
        }

        return integer;
    }

    public static AndroidJavaObject ToAndroidBigDecimal(this decimal self)
    {
        return new AndroidJavaObject("java.math.BigDecimal",
            self.ToString(CultureInfo.CreateSpecificCulture("en-US")));
    }

    public static AndroidJavaObject ToAndroidCurrency(this string self)
    {
        AndroidJavaObject currency = null;
        if (self != null)
        {
            using (AndroidJavaClass currencyClass = new AndroidJavaClass("java.util.Currency"))
            {
                currency = currencyClass.CallStatic<AndroidJavaObject>("getInstance", self);
            }
        }

        return currency;
    }

    public static AndroidJavaObject ToAndroidRevenue(this YandexAppMetricaRevenue self)
    {
        AndroidJavaObject revenue;
        using (AndroidJavaClass revenueClass = new AndroidJavaClass("com.yandex.metrica.Revenue"))
        {
            AndroidJavaObject builder;
            if (self.PriceDecimal.HasValue)
            {
                long priceMicros = decimal.ToInt64(self.PriceDecimal.Value * 1000000m);
                builder = revenueClass.CallStatic<AndroidJavaObject>("newBuilderWithMicros", priceMicros,
                    self.Currency.ToAndroidCurrency());
            }
            else
            {
                builder = revenueClass.CallStatic<AndroidJavaObject>("newBuilder", self.Price,
                    self.Currency.ToAndroidCurrency());
            }

            builder.Call<AndroidJavaObject>("withQuantity", self.Quantity.ToAndroidInteger());
            builder.Call<AndroidJavaObject>("withProductID", self.ProductID);
            builder.Call<AndroidJavaObject>("withPayload", self.Payload);
            builder.Call<AndroidJavaObject>("withReceipt", self.Receipt.ToAndroidReceipt());
            revenue = builder.Call<AndroidJavaObject>("build");
        }

        return revenue;
    }

    public static AndroidJavaObject ToAndroidAdType(this YandexAppMetricaAdRevenue.AdTypeEnum self)
    {
        AndroidJavaObject adType = null;
        using (AndroidJavaClass adTypeClass = new AndroidJavaClass("com.yandex.metrica.AdType"))
        {
            adType = adTypeClass.GetStatic<AndroidJavaObject>(self.ToString().ToUpper());
        }

        return adType;
    }

    public static AndroidJavaObject ToAndroidAdRevenue(this YandexAppMetricaAdRevenue self)
    {
        AndroidJavaObject adRevenue;
        using (AndroidJavaClass revenueClass = new AndroidJavaClass("com.yandex.metrica.AdRevenue"))
        {
            AndroidJavaObject adRevenueBigDecimal = self.AdRevenue.ToAndroidBigDecimal();
            AndroidJavaObject currency = self.Currency.ToAndroidCurrency();
            AndroidJavaObject builder = revenueClass.CallStatic<AndroidJavaObject>("newBuilder",
                adRevenueBigDecimal, currency);

            if (self.AdType.HasValue)
            {
                builder.Call<AndroidJavaObject>("withAdType", self.AdType.Value.ToAndroidAdType());
            }
            if (self.AdNetwork != null)
            {
                builder.Call<AndroidJavaObject>("withAdNetwork", self.AdNetwork);
            }
            if (self.AdUnitId != null)
            {
                builder.Call<AndroidJavaObject>("withAdUnitId", self.AdUnitId);
            }
            if (self.AdUnitName != null)
            {
                builder.Call<AndroidJavaObject>("withAdUnitName", self.AdUnitName);
            }
            if (self.AdPlacementId != null)
            {
                builder.Call<AndroidJavaObject>("withAdPlacementId", self.AdPlacementId);
            }
            if (self.AdPlacementName != null)
            {
                builder.Call<AndroidJavaObject>("withAdPlacementName", self.AdPlacementName);
            }
            if (self.Precision != null)
            {
                builder.Call<AndroidJavaObject>("withPrecision", self.Precision);
            }
            if (self.Payload != null)
            {
                builder.Call<AndroidJavaObject>("withPayload", self.Payload.ToAndroidMap());
            }

            adRevenue = builder.Call<AndroidJavaObject>("build");
        }

        return adRevenue;
    }

    public static AndroidJavaObject ToAndroidThrowable(this Exception self)
    {
        if (self == null)
        {
            return null;
        }

        string message = self.GetType() + ": " + self.Message + "\n" + self.StackTrace;
        return new AndroidJavaObject("java.lang.Throwable", message);
    }

    internal static AndroidJavaObject ToAndroidPluginErrorDetails(this YandexAppMetricaErrorDetails self)
    {
        if (self == null)
        {
            return null;
        }

        AndroidJavaObject errorDetails;
        using (AndroidJavaObject builder =
               new AndroidJavaObject("com.yandex.metrica.plugins.PluginErrorDetails$Builder"))
        {
            builder.Call<AndroidJavaObject>("withPlatform", self.Platform);
            builder.Call<AndroidJavaObject>("withVirtualMachineVersion", self.VirtualMachineVersion);

            if (self.ExceptionClass != null)
            {
                builder.Call<AndroidJavaObject>("withExceptionClass", self.ExceptionClass);
            }

            if (self.Message != null)
            {
                builder.Call<AndroidJavaObject>("withMessage", self.Message);
            }

            if (self.Stacktrace != null)
            {
                AndroidJavaObject androidStacktrace = CreateAndroidList();
                foreach (YandexAppMetricaStackTraceItem stackTraceItem in self.Stacktrace)
                {
                    AndroidJavaObject item = stackTraceItem.ToAndroidStackTraceItem();
                    if (item != null)
                    {
                        androidStacktrace.Call<bool>("add", item);
                    }
                }

                builder.Call<AndroidJavaObject>("withStacktrace", androidStacktrace);
            }

            if (self.PluginEnvironment != null)
            {
                builder.Call<AndroidJavaObject>("withPluginEnvironment", self.PluginEnvironment.ToAndroidMap());
            }

            errorDetails = builder.Call<AndroidJavaObject>("build");
        }

        return errorDetails;
    }

    private static AndroidJavaObject ToAndroidStackTraceItem(this YandexAppMetricaStackTraceItem self)
    {
        if (self == null)
        {
            return null;
        }

        AndroidJavaObject stackTraceItem;
        using (AndroidJavaObject builder = new AndroidJavaObject("com.yandex.metrica.plugins.StackTraceItem$Builder"))
        {
            if (self.ClassName != null)
            {
                builder.Call<AndroidJavaObject>("withClassName", self.ClassName);
            }

            if (self.FileName != null)
            {
                builder.Call<AndroidJavaObject>("withFileName", self.FileName);
            }

            if (self.Line != null)
            {
                builder.Call<AndroidJavaObject>("withLine", self.Line.ToAndroidInteger());
            }

            if (self.Column != null)
            {
                builder.Call<AndroidJavaObject>("withColumn", self.Column.ToAndroidInteger());
            }

            if (self.MethodName != null)
            {
                builder.Call<AndroidJavaObject>("withMethodName", self.MethodName);
            }

            stackTraceItem = builder.Call<AndroidJavaObject>("build");
        }

        return stackTraceItem;
    }

    private static AndroidJavaObject CreateAndroidList()
    {
        return new AndroidJavaObject("java.util.ArrayList");
    }

    private static AndroidJavaObject ToAndroidMap(this IDictionary<string, string> self)
    {
        AndroidJavaObject javaMap = new AndroidJavaObject("java.util.HashMap");
        IntPtr putMethod = AndroidJNIHelper.GetMethodID(javaMap.GetRawClass(), "put",
            "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

        foreach (KeyValuePair<string, string> kvp in self)
        {
            if (kvp.Key == null || kvp.Value == null)
            {
                continue; // skip null elements
            }

            using (AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key))
            {
                using (AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value))
                {
                    object[] args = new object[2];
                    args[0] = k;
                    args[1] = v;
                    AndroidJNI.CallObjectMethod(javaMap.GetRawObject(),
                        putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
                }
            }
        }

        return javaMap;
    }
}

public class YandexAppMetricaDeviceIDListenerAndroid : AndroidJavaProxy
{
    private readonly Action<string, YandexAppMetricaRequestDeviceIDError?> action;

    public YandexAppMetricaDeviceIDListenerAndroid(Action<string, YandexAppMetricaRequestDeviceIDError?> action)
        : base("com.yandex.metrica.AppMetricaDeviceIDListener")
    {
        this.action = action;
    }

    public void onLoaded(string deviceID)
    {
        action.Invoke(deviceID, null);
    }

    public void onError(AndroidJavaObject reason)
    {
        action.Invoke(null, ErrorFromAndroidReason(reason));
    }

    private static YandexAppMetricaRequestDeviceIDError? ErrorFromAndroidReason(AndroidJavaObject reason)
    {
        if (reason == null)
        {
            return null;
        }

        try
        {
            string reasonString = reason.Call<string>("toString");
            object error = Enum.Parse(typeof(YandexAppMetricaRequestDeviceIDError), reasonString);
            return (YandexAppMetricaRequestDeviceIDError?)error;
        }
        catch (ArgumentException)
        {
            return YandexAppMetricaRequestDeviceIDError.UNKNOWN;
        }
    }
}

#endif
