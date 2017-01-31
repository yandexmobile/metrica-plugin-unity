using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YandexAppMetricaDummy : BaseYandexAppMetrica
{

    #region IYandexAppMetrica implementation

    public override void ActivateWithAPIKey (string apiKey)
    {
    }

    public override void ActivateWithConfiguration (YandexAppMetricaConfig config)
    {
    }

    public override void OnResumeApplication ()
    {
    }

    public override void OnPauseApplication ()
    {
    }

    public override void ReportEvent (string message)
    {
    }

    public override void ReportEvent (string message, Dictionary<string, object> parameters)
    {
    }

    public override void ReportError (string condition, string stackTrace)
    {
    }

    public override void SetTrackLocationEnabled (bool enabled)
    {
    }

    public override void SetLocation (YandexAppMetricaConfig.Coordinates coordinates)
    {
    }

    public override void SetSessionTimeout (uint sessionTimeoutSeconds)
    {
    }

    public override void SetReportCrashesEnabled (bool enabled)
    {
    }

    public override void SetCustomAppVersion (string appVersion)
    {
    }

    public override void SetLoggingEnabled ()
    {
    }

    public override void SetEnvironmentValue (string key, string value)
    {
    }

    public override bool CollectInstalledApps { get { return false; } set { } }

    public override string LibraryVersion { get { return default(string); } }

    public override int LibraryApiLevel { get { return default(int); } }

    #endregion

}
