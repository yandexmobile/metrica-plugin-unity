using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public sealed class YandexAppMetricaConfig 
{
	public string ApiKey { get; private set; }
	public string AppVersion { get; set; }
	public Coordinates Location { get; set; }
	public int? SessionTimeout { get; set; }
	public bool? ReportCrashesEnabled { get; set; }
	public bool? TrackLocationEnabled { get; set; }
	public bool? LoggingEnabled { get; set; }
	public bool? CollectInstalledApps { get; set; }

	public YandexAppMetricaPreloadInfo PreloadInfo { get; set; }

	public YandexAppMetricaConfig(string apiKey)
	{
		ApiKey = apiKey;
	}
}

[System.Serializable]
public sealed class YandexAppMetricaPreloadInfo
{
	public string TrackingId { get; private set; }
	public Dictionary<string, string> AdditionalInfo { get; private set; }

	public YandexAppMetricaPreloadInfo(string trackingId)
	{
		TrackingId = trackingId;
		AdditionalInfo = new Dictionary<string, string>();
	}
}
