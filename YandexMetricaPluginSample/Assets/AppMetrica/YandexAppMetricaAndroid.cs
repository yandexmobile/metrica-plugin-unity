using UnityEngine;
using System.Collections;

#if UNITY_ANDROID

public class YandexAppMetricaAndroid : IYandexAppMetrica {

#region IYandexAppMetrica implementation

	private AndroidJavaClass metricaClass = null;

	public void ActivateWithAPIKey (string apiKey)
	{
		metricaClass = new AndroidJavaClass("com.yandex.metrica.YandexMetrica");
		using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			metricaClass.CallStatic("activate", playerActivityContext, apiKey);
		}
	}

	public void ActivateWithConfiguration (YandexAppMetricaConfig config)
	{
		metricaClass = new AndroidJavaClass("com.yandex.metrica.YandexMetrica");
		using (var configClass = new AndroidJavaClass("com.yandex.metrica.YandexMetricaConfig")) {
			var builder = configClass.CallStatic<AndroidJavaObject>("newConfigBuilder", config.ApiKey);

			if (config.Location != null) {
				builder.Call<AndroidJavaObject>("setLocation", config.Location.ToLocation());
			}
			if (config.AppVersion != null) {
				builder.Call<AndroidJavaObject>("setAppVersion", config.AppVersion);
			}
			if (config.TrackLocationEnabled.HasValue) {
				builder.Call<AndroidJavaObject>("setTrackLocationEnabled", config.TrackLocationEnabled.Value);
			}
			if (config.SessionTimeout.HasValue) {
				builder.Call<AndroidJavaObject>("setSessionTimeout", config.SessionTimeout.Value);
			}
			if (config.ReportCrashesEnabled.HasValue) {
				builder.Call<AndroidJavaObject>("setReportCrashesEnabled", config.ReportCrashesEnabled.Value);
			}
			if (config.LoggingEnabled ?? false) {
				builder.Call<AndroidJavaObject>("setLogEnabled");
			}
			if (config.CollectInstalledApps.HasValue) {
				builder.Call<AndroidJavaObject>("setCollectInstalledApps", config.CollectInstalledApps.Value);
			}
			if (config.PreloadInfo != null) {
				var preloadInfoClass = new AndroidJavaClass("com.yandex.metrica.PreloadInfo");
				var preloadInfoBuilder = preloadInfoClass.CallStatic<AndroidJavaObject>("newBuilder", config.PreloadInfo.TrackingId);
				foreach (var kvp in config.PreloadInfo.AdditionalInfo) {
					preloadInfoBuilder.Call<AndroidJavaObject>("setAdditionalParams", kvp.Key, kvp.Value);
				}
				builder.Call<AndroidJavaObject>("setPreloadInfo", preloadInfoBuilder.Call<AndroidJavaObject>("build"));
			}

			// Native crashes are currently not supported
			builder.Call<AndroidJavaObject>("setReportNativeCrashesEnabled", false);

			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
				metricaClass.CallStatic("activate", playerActivityContext, builder.Call<AndroidJavaObject>("build"));
			}
		}
	}

	public void OnResumeApplication ()
	{
		if (metricaClass != null) {
			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
				metricaClass.CallStatic("onResumeActivity", playerActivityContext);
			}
		}
	}
	
	public void OnPauseApplication ()
	{
		if (metricaClass != null) {
			using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
				metricaClass.CallStatic("onPauseActivity", playerActivityContext);
			}
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

	public void ReportError (string condition, string stackTrace)
	{
		if (metricaClass != null) {
			var throwableObject = new AndroidJavaObject("java.lang.Throwable", "\n" + stackTrace);
			metricaClass.CallStatic("reportError", condition, throwableObject);
		}
	}

	public void SetTrackLocationEnabled (bool enabled) 
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setTrackLocationEnabled", enabled);
		}
	}
	
	public void SetLocation (Coordinates coordinates)
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setLocation", coordinates.ToLocation());
		}
	}

	public void SetSessionTimeout (uint sessionTimeoutSeconds) 
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setSessionTimeout", (int)sessionTimeoutSeconds);
		}
	}

	public void SetReportCrashesEnabled (bool enabled) 
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setReportCrashesEnabled", enabled);
		}
	}

	public void SetCustomAppVersion (string appVersion)
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setCustomAppVersion", appVersion);
		}
	}

	public void SetLoggingEnabled ()
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setLoggingEnabled");
		}
	}
	
	public void SetEnvironmentValue (string key, string value)
	{
		if (metricaClass != null) {
			metricaClass.CallStatic("setEnvironmentValue", key, value);
		}
	}

	public bool CollectInstalledApps { 
		get { 
			if (metricaClass != null) {
				return metricaClass.CallStatic<bool>("getCollectInstalledApps");
			}
			return false;
		} 
		set {
			if (metricaClass != null) {
				metricaClass.CallStatic("setCollectInstalledApps", value);
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

public static class YandexAppMetricaExtensionsAndroid 
{
	public static AndroidJavaObject ToLocation (this Coordinates self)
	{
		AndroidJavaObject location = new AndroidJavaObject("android.location.Location", "");
		location.Call("setLatitude", self.Latitude);
		location.Call("setLongitude", self.Longitude);
		return location;
	}
}

#endif
