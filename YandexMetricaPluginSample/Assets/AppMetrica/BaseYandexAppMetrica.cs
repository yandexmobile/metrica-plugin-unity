using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseYandexAppMetrica : IYandexAppMetrica
{
    private YandexAppMetricaConfig? _metricaConfig;

    public event ConfigUpdateHandler OnActivation;

    public YandexAppMetricaConfig? ActivationConfig {
        get {
            return _metricaConfig;
        }
    }

    public virtual void ActivateWithAPIKey (string apiKey)
    {
        UpdateConfiguration (new YandexAppMetricaConfig (apiKey));
    }

    public virtual void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
        UpdateConfiguration (config);
    }

    private void UpdateConfiguration (YandexAppMetricaConfig config)
    {
        _metricaConfig = config;
        ConfigUpdateHandler receiver = OnActivation;
        if (receiver != null) {
            receiver (config);
        }
    }

    public abstract void OnResumeApplication ();

    public abstract void OnPauseApplication ();

    public abstract void ReportEvent (string message);

    public abstract void ReportEvent (string message, Dictionary<string, object> parameters);

    public abstract void ReportError (string condition, string stackTrace);

    public abstract void SetTrackLocationEnabled (bool enabled);

    public abstract void SetLocation (YandexAppMetricaConfig.Coordinates coordinates);

    public abstract void SetSessionTimeout (uint sessionTimeoutSeconds);

    public abstract void SetReportCrashesEnabled (bool enabled);

    public abstract void SetCustomAppVersion (string appVersion);

    public abstract void SetLoggingEnabled ();

    public abstract void SetEnvironmentValue (string key, string value);

    public abstract bool CollectInstalledApps { get; set; }

    public abstract string LibraryVersion { get; }

    public abstract int LibraryApiLevel { get; }

}
