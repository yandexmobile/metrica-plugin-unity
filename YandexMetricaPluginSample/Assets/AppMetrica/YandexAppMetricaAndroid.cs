using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID

public class YandexAppMetricaAndroid : BaseYandexAppMetrica
{


#region IYandexAppMetrica implementation

    private AndroidJavaClass metricaClass = null;

    public override void ActivateWithAPIKey (string apiKey)
    {
        base.ActivateWithAPIKey (apiKey);
        metricaClass = new AndroidJavaClass ("com.yandex.metrica.YandexMetrica");
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
            metricaClass.CallStatic ("activate", playerActivityContext, apiKey);
        }
    }

    public override void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
        base.ActivateWithConfiguration (config);
        metricaClass = new AndroidJavaClass ("com.yandex.metrica.YandexMetrica");
        using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
            var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
            metricaClass.CallStatic ("activate", playerActivityContext, config.ToAndroidAppMetricaConfig (metricaClass));
        }
    }

    public override void OnResumeApplication ()
    {
        if (metricaClass != null) {
            using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
                var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
                metricaClass.CallStatic ("onResumeActivity", playerActivityContext);
            }
        }
    }

    public override void OnPauseApplication ()
    {
        if (metricaClass != null) {
            using (var activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
                var playerActivityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
                metricaClass.CallStatic ("onPauseActivity", playerActivityContext);
            }
        }
    }

    public override void ReportEvent (string message)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("reportEvent", message);
        }
    }

    public override void ReportEvent (string message, Dictionary<string, object> parameters)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("reportEvent", message, YMMJSONUtils.JSONEncoder.Encode (parameters));
        }
    }

    public override void ReportError (string condition, string stackTrace)
    {
        if (metricaClass != null) {
            var throwableObject = new AndroidJavaObject ("java.lang.Throwable", "\n" + stackTrace);
            metricaClass.CallStatic ("reportError", condition, throwableObject);
        }
    }

    public override void SetTrackLocationEnabled (bool enabled)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("setTrackLocationEnabled", enabled);
        }
    }

    public override void SetLocation (YandexAppMetricaConfig.Coordinates coordinates)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("setLocation", coordinates.ToAndroidLocation ());
        }
    }

    public override void SetSessionTimeout (uint sessionTimeoutSeconds)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("setSessionTimeout", (int)sessionTimeoutSeconds);
        }
    }

    public override void SetReportCrashesEnabled (bool enabled)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("setReportCrashesEnabled", enabled);
        }
    }

    public override void SetCustomAppVersion (string appVersion)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("setCustomAppVersion", appVersion);
        }
    }

    public override void SetLoggingEnabled ()
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("setLoggingEnabled");
        }
    }

    public override void SetEnvironmentValue (string key, string value)
    {
        if (metricaClass != null) {
            metricaClass.CallStatic ("setEnvironmentValue", key, value);
        }
    }

    public override bool CollectInstalledApps {
        get {
            if (metricaClass != null) {
                return metricaClass.CallStatic<bool> ("getCollectInstalledApps");
            }
            return false;
        }
        set {
            if (metricaClass != null) {
                metricaClass.CallStatic ("setCollectInstalledApps", value);
            }
        }
    }

    public override int LibraryApiLevel {
        get {
            if (metricaClass != null) {
                return metricaClass.CallStatic<int> ("getLibraryApiLevel");
            }
            return 0;
        }
    }

    public override string LibraryVersion {
        get {
            if (metricaClass != null) {
                return metricaClass.CallStatic<string> ("getLibraryVersion");
            }
            return null;
        }
    }



#endregion

}

public static class YandexAppMetricaExtensionsAndroid
{
    public static AndroidJavaObject ToAndroidAppMetricaConfig (this YandexAppMetricaConfig self, AndroidJavaClass metricaClass)
    {
        AndroidJavaObject appMetricaConfig = null;
        using (var configClass = new AndroidJavaClass ("com.yandex.metrica.YandexMetricaConfig")) {
            var builder = configClass.CallStatic<AndroidJavaObject> ("newConfigBuilder", self.ApiKey);

            if (self.Location.HasValue) {
                var location = self.Location.Value;
                builder.Call<AndroidJavaObject> ("setLocation", location.ToAndroidLocation ());
            }
            if (self.AppVersion != null) {
                builder.Call<AndroidJavaObject> ("setAppVersion", self.AppVersion);
            }
            if (self.TrackLocationEnabled.HasValue) {
                builder.Call<AndroidJavaObject> ("setTrackLocationEnabled", self.TrackLocationEnabled.Value);
            }
            if (self.SessionTimeout.HasValue) {
                builder.Call<AndroidJavaObject> ("setSessionTimeout", self.SessionTimeout.Value);
            }
            if (self.ReportCrashesEnabled.HasValue) {
                builder.Call<AndroidJavaObject> ("setReportCrashesEnabled", self.ReportCrashesEnabled.Value);
            }
            if (self.LoggingEnabled ?? false) {
                builder.Call<AndroidJavaObject> ("setLogEnabled");
            }
            if (self.CollectInstalledApps.HasValue) {
                builder.Call<AndroidJavaObject> ("setCollectInstalledApps", self.CollectInstalledApps.Value);
            }
            if (self.HandleFirstActivationAsUpdateEnabled.HasValue) {
                builder.Call<AndroidJavaObject> ("handleFirstActivationAsUpdate", self.HandleFirstActivationAsUpdateEnabled.Value);
            }
            if (self.PreloadInfo.HasValue) {
                var preloadInfo = self.PreloadInfo.Value;
                var preloadInfoClass = new AndroidJavaClass ("com.yandex.metrica.PreloadInfo");
                var preloadInfoBuilder = preloadInfoClass.CallStatic<AndroidJavaObject> ("newBuilder", preloadInfo.TrackingId);
                foreach (var kvp in preloadInfo.AdditionalInfo) {
                    preloadInfoBuilder.Call<AndroidJavaObject> ("setAdditionalParams", kvp.Key, kvp.Value);
                }
                builder.Call<AndroidJavaObject> ("setPreloadInfo", preloadInfoBuilder.Call<AndroidJavaObject> ("build"));
            }

            // Native crashes are currently not supported
            builder.Call<AndroidJavaObject> ("setReportNativeCrashesEnabled", false);
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
}

#endif
