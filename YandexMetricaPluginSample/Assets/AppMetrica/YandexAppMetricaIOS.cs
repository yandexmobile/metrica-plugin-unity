using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IPHONE || UNITY_IOS

public class YandexAppMetricaIOS : IYandexAppMetrica 
{
	[DllImport("__Internal")]
	private static extern void ymm_startWithAPIKey(string apiKey);

	[DllImport("__Internal")]
	private static extern void ymm_reportEvent(string message);

	[DllImport("__Internal")]
	private static extern void ymm_reportEventWithParameters(string message, string parameters);

	[DllImport("__Internal")]
	private static extern void ymm_reportError(string condition, string stackTrace);

	[DllImport("__Internal")]
	private static extern void ymm_sendEventsBuffer();

	[DllImport("__Internal")]
	private static extern void ymm_startNewSessionManually();

	[DllImport("__Internal")]
	private static extern void ymm_setTrackLocationEnabled(bool enabled);

	[DllImport("__Internal")]
	private static extern bool ymm_isTrackLocationEnabled();

	[DllImport("__Internal")]
	private static extern void ymm_setLocation(double latitude, double longitude);

	[DllImport("__Internal")]
	private static extern void ymm_setDispatchPeriod(uint dispatchPeriodSeconds);

	[DllImport("__Internal")]
	private static extern uint ymm_dispatchPeriod();

	[DllImport("__Internal")]
	private static extern void ymm_setMaxReportsCount(uint maxReportsCount);

	[DllImport("__Internal")]
	private static extern uint ymm_maxReportsCount();

	[DllImport("__Internal")]
	private static extern void ymm_setSessionTimeout(uint sessionTimeoutSeconds);
	
	[DllImport("__Internal")]
	private static extern uint ymm_sessionTimeout();

	[DllImport("__Internal")]
	private static extern void ymm_setReportsEnabled(bool enabled);

	[DllImport("__Internal")]
	private static extern bool ymm_isReportsEnabled();
	
	[DllImport("__Internal")]
	private static extern void ymm_setReportCrashesEnabled(bool enabled);

	[DllImport("__Internal")]
	private static extern bool ymm_isReportCrashesEnabled();
	
	[DllImport("__Internal")]
	private static extern void ymm_setCustomAppVersion(string appVersion);
	
	[DllImport("__Internal")]
	private static extern void ymm_setLogLevel(uint level);
	
	[DllImport("__Internal")]
	private static extern void ymm_setEnvironmentValue(string key, string value);

	[DllImport("__Internal")]
	private static extern string ymm_getLibraryVersion ();

#region IYandexMobileMetrica implementation

	public void StartWithAPIKey (string apiKey)
	{
		ymm_startWithAPIKey(apiKey);
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

	public void SendEventsBuffer ()
	{
		ymm_sendEventsBuffer();
	}

	public void StartNewSessionManually ()
	{
		ymm_startNewSessionManually();
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
	
	public void SetLogLevel (uint logLevel)
	{
		ymm_setLogLevel(logLevel);
	}
	
	public void SetEnvironmentValue (string key, string value)
	{
		ymm_setEnvironmentValue(key, value);
	}
	
	public bool TrackLocationEnabled {
		get {
			return ymm_isTrackLocationEnabled();
		}
		set {
			ymm_setTrackLocationEnabled(value);
		}
	}
	public uint DispatchPeriod {
		get {
			return ymm_dispatchPeriod();
		}
		set {
			ymm_setDispatchPeriod(value);
		}
	}

	public uint MaxReportsCount {
		get {
			return ymm_maxReportsCount();
		}
		set {
			ymm_setMaxReportsCount(value);
		}
	}

	public uint SessionTimeout {
		get {
			return ymm_sessionTimeout();
		}
		set {
			ymm_setSessionTimeout(value);
		}
	}
	
	public bool ReportsEnabled {
		get {
			return ymm_isReportsEnabled();
		}
		set {
			ymm_setReportsEnabled(value);
		}
	}

	public bool ReportCrashesEnabled {
		get {
			return ymm_isReportCrashesEnabled();
		}
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
