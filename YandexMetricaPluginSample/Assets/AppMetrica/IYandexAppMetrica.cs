using UnityEngine;
using System.Collections;

public interface IYandexAppMetrica
{
	void StartWithAPIKey(string apiKey);

	void ReportEvent(string message);
	void ReportEvent(string message, Hashtable parameters);
	void ReportError(string condition, string stackTrace);

	void SendEventsBuffer();
	void StartNewSessionManually();
	void OnResumeApplication();
	void OnPauseApplication();

	void SetLocation(LocationInfo locationInfo);
	void SetCustomAppVersion(string appVersion);
	void SetLogLevel(uint logLevel);
	void SetEnvironmentValue(string key, string value);

	bool TrackLocationEnabled { get; set; }
	uint DispatchPeriod { get; set; }
	uint MaxReportsCount { get; set; }
	uint SessionTimeout { get; set; }
	bool ReportsEnabled { get; set; }
	bool ReportCrashesEnabled { get; set; }

	int LibraryApiLevel { get; }
	string LibraryVersion { get; }
}
