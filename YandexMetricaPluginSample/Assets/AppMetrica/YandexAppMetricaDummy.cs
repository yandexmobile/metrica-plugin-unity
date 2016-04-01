using UnityEngine;
using System.Collections;

public class YandexAppMetricaDummy : IYandexAppMetrica {

#region IYandexAppMetrica implementation

	public void ActivateWithAPIKey (string apiKey) { }

	public void ActivateWithConfiguration (YandexAppMetricaConfig config) { }

	public void OnResumeApplication () { }

	public void OnPauseApplication () { }

	public void ReportEvent (string message) { }
	
	public void ReportEvent (string message, Hashtable parameters) { } 
	
	public void ReportError (string condition, string stackTrace) { }
	
	public void SetTrackLocationEnabled (bool enabled) { }
	
	public void SetLocation (Coordinates coordinates) { }
	
	public void SetSessionTimeout (uint sessionTimeoutSeconds) { }
	
	public void SetReportCrashesEnabled (bool enabled) { }
	
	public void SetCustomAppVersion (string appVersion) { }
	
	public void SetLoggingEnabled () { }
	
	public void SetEnvironmentValue (string key, string value) { }
	
	public bool CollectInstalledApps { get { return false; } set { } }
	
	public string LibraryVersion { get { return default(string); } }
	
	public int LibraryApiLevel { get { return default(int); } }

#endregion

}
