using UnityEngine;
using System.Collections;

public class YandexAppMetricaDummy : IYandexAppMetrica {

#region IYandexMobileMetrica implementation

	public void ActivateWithAPIKey (string apiKey)
	{
	}

	public void ReportEvent (string message)
	{
	}

	public void ReportEvent (string message, Hashtable parameters)
	{
	}

	public void ReportError (string condition, string stackTrace)
	{
	}

	public void OnResumeApplication()
	{
	}

	public void OnPauseApplication()
	{
	}

	public void SetLocation (LocationInfo locationInfo)
	{
	}
	
	public void SetCustomAppVersion (string appVersion)
	{
	}
	
	public void SetLogLevel (uint logLevel)
	{
	}
	
	public void SetEnvironmentValue (string key, string value)
	{
	}

	public bool TrackLocationEnabled {
		set {
		}
	}

	public uint SessionTimeout {
		set {
		}
	}
	
	public bool ReportCrashesEnabled {
		set {
		}
	}

	
	public int LibraryApiLevel {
		get {
			return 0;
		}
	}
	
	public string LibraryVersion {
		get {
			return null;
		}
	}

#endregion

}
