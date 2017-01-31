using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_IPHONE || UNITY_IOS

public class YandexAppMetricaIOS : BaseYandexAppMetrica
{
    [DllImport ("__Internal")]
    private static extern void ymm_activateWithAPIKey (string apiKey);

    [DllImport ("__Internal")]
    private static extern void ymm_activateWithConfigurationJSON (string configurationJSON);

    [DllImport ("__Internal")]
    private static extern void ymm_reportEvent (string message);

    [DllImport ("__Internal")]
    private static extern void ymm_reportEventWithParameters (string message, string parameters);

    [DllImport ("__Internal")]
    private static extern void ymm_reportError (string condition, string stackTrace);

    [DllImport ("__Internal")]
    private static extern void ymm_setTrackLocationEnabled (bool enabled);

    [DllImport ("__Internal")]
    private static extern void ymm_setLocation (double latitude, double longitude);

    [DllImport ("__Internal")]
    private static extern void ymm_setSessionTimeout (uint sessionTimeoutSeconds);

    [DllImport ("__Internal")]
    private static extern void ymm_setReportCrashesEnabled (bool enabled);

    [DllImport ("__Internal")]
    private static extern void ymm_setCustomAppVersion (string appVersion);

    [DllImport ("__Internal")]
    private static extern void ymm_setLoggingEnabled (bool enabled);

    [DllImport ("__Internal")]
    private static extern void ymm_setEnvironmentValue (string key, string value);

    [DllImport ("__Internal")]
    private static extern string ymm_getLibraryVersion ();

    #region IYandexAppMetrica implementation

    public override void ActivateWithAPIKey (string apiKey)
    {
        base.ActivateWithAPIKey (apiKey);
        ymm_activateWithAPIKey (apiKey);
    }

    public override void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
        base.ActivateWithConfiguration (config);
        ymm_activateWithConfigurationJSON (YMMJSONUtils.JSONEncoder.Encode (config.ToHashtable ()));
    }

    public override void OnResumeApplication ()
    {
        // It does nothing for iOS
    }

    public override void OnPauseApplication ()
    {
        // It does nothing for iOS
    }

    public override void ReportEvent (string message)
    {
        ymm_reportEvent (message);
    }

    public override void ReportEvent (string message, Dictionary<string, object> parameters)
    {
        ymm_reportEventWithParameters (message, YMMJSONUtils.JSONEncoder.Encode (parameters));
    }

    public override void ReportError (string condition, string stackTrace)
    {
        ymm_reportError (condition, stackTrace);
    }

    public override void SetTrackLocationEnabled (bool enabled)
    {
        ymm_setTrackLocationEnabled (enabled);
    }

    public override void SetLocation (YandexAppMetricaConfig.Coordinates coordinates)
    {
        ymm_setLocation (coordinates.Latitude, coordinates.Longitude);
    }

    public override void SetSessionTimeout (uint sessionTimeoutSeconds)
    {
        ymm_setSessionTimeout (sessionTimeoutSeconds);
    }

    public override void SetReportCrashesEnabled (bool enabled)
    {
        ymm_setReportCrashesEnabled (enabled);
    }

    public override void SetCustomAppVersion (string appVersion)
    {
        ymm_setCustomAppVersion (appVersion);
    }

    public override void SetLoggingEnabled ()
    {
        ymm_setLoggingEnabled (true);
    }

    public override void SetEnvironmentValue (string key, string value)
    {
        ymm_setEnvironmentValue (key, value);
    }

    public override bool CollectInstalledApps {
        get {
            // Not available for iOS
            return false;
        }
        set {
            // Not available for iOS
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

    #endregion

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
        if (self.ReportCrashesEnabled.HasValue) {
            data ["ReportCrashesEnabled"] = self.ReportCrashesEnabled.Value;
        }
        if (self.TrackLocationEnabled.HasValue) {
            data ["TrackLocationEnabled"] = self.TrackLocationEnabled.Value;
        }
        if (self.LoggingEnabled.HasValue) {
            data ["LoggingEnabled"] = self.LoggingEnabled.Value;
        }
        if (self.HandleFirstActivationAsUpdateEnabled.HasValue) {
            data ["HandleFirstActivationAsUpdateEnabled"] = self.HandleFirstActivationAsUpdateEnabled.Value;
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
}

#endif
