using UnityEngine;
using System.Collections;

public interface IYandexAppMetrica
{
	void ActivateWithAPIKey(string apiKey);

	void ReportEvent(string message);
	void ReportEvent(string message, Hashtable parameters);
	void ReportError(string condition, string stackTrace);

	void OnResumeApplication();
	void OnPauseApplication();

	void SetLocation(LocationInfo locationInfo);
	void SetCustomAppVersion(string appVersion);
	void SetEnvironmentValue(string key, string value);

	bool TrackLocationEnabled { set; }
	uint SessionTimeout { set; }
	bool ReportCrashesEnabled { set; }

	int LibraryApiLevel { get; }
	string LibraryVersion { get; }
}
