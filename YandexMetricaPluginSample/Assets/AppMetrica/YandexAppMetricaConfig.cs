using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct YandexAppMetricaConfig
{
    [System.Serializable]
    public struct Coordinates
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public string ApiKey { get; private set; }

    public string AppVersion { get; set; }

    public Coordinates? Location { get; set; }

    public int? SessionTimeout { get; set; }

    public bool? ReportCrashesEnabled { get; set; }

    public bool? TrackLocationEnabled { get; set; }

    public bool? LoggingEnabled { get; set; }

    public bool? CollectInstalledApps { get; set; }

    public bool? HandleFirstActivationAsUpdateEnabled { get; set; }

    public YandexAppMetricaPreloadInfo? PreloadInfo { get; set; }

    public YandexAppMetricaConfig (string apiKey)
    {
        ApiKey = apiKey;
        AppVersion = null;
        Location = null;
        SessionTimeout = null;
        ReportCrashesEnabled = null;
        TrackLocationEnabled = null;
        LoggingEnabled = null;
        CollectInstalledApps = null;
        HandleFirstActivationAsUpdateEnabled = null;
        PreloadInfo = null;
    }
}

[System.Serializable]
public struct YandexAppMetricaPreloadInfo
{
    public string TrackingId { get; private set; }

    public Dictionary<string, string> AdditionalInfo { get; private set; }

    public YandexAppMetricaPreloadInfo (string trackingId)
    {
        TrackingId = trackingId;
        AdditionalInfo = new Dictionary<string, string> ();
    }
}
