using UnityEngine;
using System.Collections;

#if UNITY_ANDROID

public class YandexAppMetricaAndroid : IYandexAppMetrica {

#region IYandexMobileMetrica implementation

	private AndroidJavaClass metricaClass = null;

	public void StartWithAPIKey (string apiKey)
	{
		metricaClass = new AndroidJavaClass("com.yandex.metrica.YandexMetrica");
		using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			playerActivityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
				metricaClass.CallStatic("initialize", playerActivityContext, apiKey);
			}));
		}
	}

	public void ReportEvent (string message)
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("reportEvent", message);
		}
	}

	public void ReportEvent (string message, Hashtable parameters)
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("reportEvent", message, YMMJSONUtils.JSONEncoder.Encode(parameters));
		}
	}

	public void ReportError(string condition, string stackTrace)
	{
		if (metricaClass != null) {
			var throwableObject = new AndroidJavaObject("java.lang.Throwable", "\n" + stackTrace);
			metricaClass.CallStatic("reportError", condition, throwableObject);
		}
	}

	public void SendEventsBuffer ()
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("sendEventsBuffer");
		}
	}

	public void StartNewSessionManually ()
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("startNewSessionManually");
		}
	}

	public void OnResumeApplication()
	{
		if (metricaClass != null) {
			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
				metricaClass.CallStatic("onResumeActivity", playerActivityContext);
			}
		}
	}

	public void OnPauseApplication()
	{
		if (metricaClass != null) {
			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
				metricaClass.CallStatic("onPauseActivity", playerActivityContext);
			}
		}
	}

	public void SetLocation (LocationInfo locationInfo)
	{
		if (metricaClass != null) {
			AndroidJavaObject location = new AndroidJavaObject("android.location.Location", "");
			location.Call("setLatitude", locationInfo.latitude);
			location.Call("setLongitude", locationInfo.longitude);
			metricaClass.CallStatic("setLocation", location);
		}
	}

	public void SetCustomAppVersion (string appVersion)
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setCustomAppVersion", appVersion);
		}
	}
	
	public void SetLogLevel (uint logLevel)
	{
		// Not available for Android
	}
	
	public void SetEnvironmentValue (string key, string value)
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setEnvironmentValue", key, value);
		}
	}

	public bool TrackLocationEnabled {
		get {
			// Not available for Android
			return false;
		}
		set {
			if (metricaClass != null) {
				metricaClass.CallStatic("setTrackLocationEnabled", value);
			}
		}
	}
	
	public uint DispatchPeriod {
		get {
			// Not available for Android
			return 0;
		}
		set {
			if (metricaClass != null) {
				metricaClass.CallStatic("setDispatchPeriod", (int)value);
			}
		}
	}
	
	public uint MaxReportsCount {
		get {
			// Not available for Android
			return 0;
		}
		set {
			if (metricaClass != null) {
				metricaClass.CallStatic("setMaxReportsCount", (int)value);
			}
		}
	}

	public uint SessionTimeout {
		get {
			// Not available for Android
			return 0;
		}
		set {
			if (metricaClass != null) {
				metricaClass.CallStatic("setSessionTimeout", (int)value);
			}
		}
	}
	
	public bool ReportsEnabled {
		get {
			// Not available for Android
			return false;
		}
		set {
			if (metricaClass != null) {
				metricaClass.CallStatic("setReportsEnabled", value);
			}
		}
	}
	
	public bool ReportCrashesEnabled {
		get {
			// Not available for Android
			return false;
		}
		set {
			if (metricaClass != null) {
				metricaClass.CallStatic("setReportCrashesEnabled", value);
			}
		}
	}

	public int LibraryApiLevel {
		get {
			if (metricaClass != null) {
				return metricaClass.CallStatic<int>("getLibraryApiLevel");
			}
			return 0;
		}
	}
	
	public string LibraryVersion {
		get {
			if (metricaClass != null) {
				return metricaClass.CallStatic<string>("getLibraryVersion");
			}
			return null;
		}
	}

#endregion

}

#endif
