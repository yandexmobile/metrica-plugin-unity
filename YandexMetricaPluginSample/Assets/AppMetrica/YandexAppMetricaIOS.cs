using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IPHONE || UNITY_IOS

public class YandexAppMetricaIOS : IYandexAppMetrica 
{
	[DllImport("__Internal")]
	private static extern void ymm_activateWithAPIKey(string apiKey);

	[DllImport("__Internal")]
	private static extern void ymm_activateWithConfigurationJSON(string configurationJSON);

	[DllImport("__Internal")]
	private static extern void ymm_reportEvent(string message);

	[DllImport("__Internal")]
	private static extern void ymm_reportEventWithParameters(string message, string parameters);

	[DllImport("__Internal")]
	private static extern void ymm_reportError(string condition, string stackTrace);
	
	[DllImport("__Internal")]
	private static extern void ymm_setTrackLocationEnabled(bool enabled);

	[DllImport("__Internal")]
	private static extern void ymm_setLocation(double latitude, double longitude);

	[DllImport("__Internal")]
	private static extern void ymm_setSessionTimeout(uint sessionTimeoutSeconds);

	[DllImport("__Internal")]
	private static extern void ymm_setReportCrashesEnabled(bool enabled);
	
	[DllImport("__Internal")]
	private static extern void ymm_setCustomAppVersion(string appVersion);

	[DllImport("__Internal")]
	private static extern void ymm_setLoggingEnabled(bool enabled);
	
	[DllImport("__Internal")]
	private static extern void ymm_setEnvironmentValue(string key, string value);

	[DllImport("__Internal")]
	private static extern string ymm_getLibraryVersion ();

#region IYandexAppMetrica implementation

	public void ActivateWithAPIKey (string apiKey)
	{
		ymm_activateWithAPIKey(apiKey);
	}

	public void ActivateWithConfiguration (YandexAppMetricaConfig config)
	{
		ymm_activateWithConfigurationJSON(YMMJSONUtils.JSONEncoder.Encode(config.ToHashtable()));
	}

	public void OnResumeApplication ()
	{
		// It does nothing for iOS
	}
	
	public void OnPauseApplication ()
	{
		// It does nothing for iOS
	}

	public void ReportEvent (string message)
	{
		ymm_reportEvent(message);
	}

	public void ReportEvent (string message, Hashtable parameters)
	{
		ymm_reportEventWithParameters(message, YMMJSONUtils.JSONEncoder.Encode(parameters));
	}

	public void ReportError (string condition, string stackTrace)
	{
		ymm_reportError(condition, stackTrace);
	}

	public void SetTrackLocationEnabled (bool enabled)
	{
		ymm_setTrackLocationEnabled(enabled);
	}
	
	public void SetLocation (Coordinates coordinates)
	{
		ymm_setLocation(coordinates.Latitude, coordinates.Longitude);
	}
	
	public void SetSessionTimeout (uint sessionTimeoutSeconds)
	{
		ymm_setSessionTimeout(sessionTimeoutSeconds);
	}
	
	public void SetReportCrashesEnabled (bool enabled)
	{
		ymm_setReportCrashesEnabled(enabled);
	}
	
	public void SetCustomAppVersion (string appVersion)
	{
		ymm_setCustomAppVersion(appVersion);
	}
	
	public void SetLoggingEnabled ()
	{
		ymm_setLoggingEnabled(true);
	}
	
	public void SetEnvironmentValue (string key, string value)
	{
		ymm_setEnvironmentValue(key, value);
	}

	public bool CollectInstalledApps { 
		get { 
			// Not available for iOS
			return false; 
		} 
		set {
			// Not available for iOS
		}
	}

	public int LibraryApiLevel {
		get {
			// Not available for iOS
			return 0;
		}
	}
	
	public string LibraryVersion {
		get {
			return ymm_getLibraryVersion();
		}
	}
	
#endregion

}

public static class YandexAppMetricaExtensionsIOS
{
	public static Hashtable ToHashtable(this YandexAppMetricaConfig self)
	{
		var data = new Hashtable {
			{ "ApiKey", self.ApiKey },
		};

		if (self.AppVersion != null) {
			data["AppVersion"] = self.AppVersion;
		}
		if (self.Location != null) {
			data["Location"] = new Hashtable {
				{ "Latitude", self.Location.Latitude},
				{ "Longitude", self.Location.Longitude},
			};
		}
		if (self.SessionTimeout.HasValue) {
			data["SessionTimeout"] = self.SessionTimeout.Value;
		}
		if (self.ReportCrashesEnabled.HasValue) {
			data["ReportCrashesEnabled"] = self.ReportCrashesEnabled.Value;
		}
		if (self.TrackLocationEnabled.HasValue) {
			data["TrackLocationEnabled"] = self.TrackLocationEnabled.Value;
		}
		if (self.LoggingEnabled.HasValue) {
			data["LoggingEnabled"] = self.LoggingEnabled.Value;
		}

		if (self.PreloadInfo != null) {
			data["PreloadInfo"] = new Hashtable {
				{ "TrackingId", self.PreloadInfo.TrackingId },
				{ "AdditionalInfo", new Hashtable(self.PreloadInfo.AdditionalInfo) },
			};
		}

		return data;
	}
}

#endif
