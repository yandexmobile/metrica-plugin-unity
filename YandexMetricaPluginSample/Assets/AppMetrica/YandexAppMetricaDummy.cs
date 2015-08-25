using UnityEngine;
using System.Collections;

public class YandexAppMetricaDummy : IYandexAppMetrica {

#region IYandexMobileMetrica implementation

	public void StartWithAPIKey (string apiKey)
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

	public void SendEventsBuffer ()
	{
	}

	public void StartNewSessionManually ()
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
		get {
			return false;
		}
		set {
		}
	}

	public uint DispatchPeriod {
		get {
			return 0;
		}
		set {
		}
	}

	public uint MaxReportsCount {
		get {
			return 0;
		}
		set {
		}
	}

	public uint SessionTimeout {
		get {
			return 0;
		}
		set {
		}
	}

	public bool ReportsEnabled {
		get {
			return false;
		}
		set {
		}
	}
	
	public bool ReportCrashesEnabled {
		get {
			return false;
		}
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
