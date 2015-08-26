using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IPHONE || UNITY_IOS

public class YandexAppMetricaIOS : IYandexAppMetrica 
{
	[DllImport("__Internal")]
	private static extern void ymm_activateWithAPIKey(string apiKey);

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
	private static extern void ymm_setEnvironmentValue(string key, string value);

	[DllImport("__Internal")]
	private static extern string ymm_getLibraryVersion ();

#region IYandexMobileMetrica implementation

	public void ActivateWithAPIKey (string apiKey)
	{
		ymm_activateWithAPIKey(apiKey);
	}

	public void ReportEvent (string message)
	{
		ymm_reportEvent(message);
	}

	public void ReportEvent (string message, Hashtable parameters)
	{
		ymm_reportEventWithParameters(message, YMMJSONUtils.JSONEncoder.Encode(parameters));
	}

	public void ReportError(string condition, string stackTrace)
	{
		ymm_reportError(condition, stackTrace);
	}

	public void OnResumeApplication()
	{
		// It does nothing for iOS
	}

	public void OnPauseApplication()
	{
		// It does nothing for iOS
	}

	public void SetLocation (LocationInfo locationInfo)
	{
		ymm_setLocation(locationInfo.latitude, locationInfo.longitude);
	}
	
	public void SetCustomAppVersion (string appVersion)
	{
		ymm_setCustomAppVersion(appVersion);
	}
	
	public void SetEnvironmentValue (string key, string value)
	{
		ymm_setEnvironmentValue(key, value);
	}
	
	public bool TrackLocationEnabled {
		set {
			ymm_setTrackLocationEnabled(value);
		}
	}

	public uint SessionTimeout {
		set {
			ymm_setSessionTimeout(value);
		}
	}

	public bool ReportCrashesEnabled {
		set {
			ymm_setReportCrashesEnabled(value);
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

#endif
