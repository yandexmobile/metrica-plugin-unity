// Uncomment the following line to disable location tracking
// #define APP_METRICA_TRACK_LOCATION_DISABLED
// or just add APP_METRICA_TRACK_LOCATION_DISABLED into
// Player Settings -> Other Settings -> Scripting Define Symbols

using UnityEngine;
using System.Collections;

public class AppMetrica : MonoBehaviour
{
	private class LogEvent 
	{
		public string Condition;
		public string StackTrace;
	}

	[SerializeField]
	private string APIKey;
	
	[SerializeField]
	private bool ExceptionsReporting = true;

	[SerializeField]
	private uint SessionTimeoutSec = 10;

#if !APP_METRICA_TRACK_LOCATION_DISABLED
	[SerializeField]
	private bool TrackLocation = true;
#endif

	[SerializeField]
	private bool LoggingEnabled = true;
	
	private static bool _isInitialized = false;
	private ArrayList _handledLogEvents = new ArrayList();
	private bool _actualPauseStatus = false;

	private static IYandexAppMetrica _metrica = null;
	private static object syncRoot = new Object();
	public static IYandexAppMetrica Instance 
	{
		get {
			if (_metrica == null) {
				lock (syncRoot) {
#if UNITY_IPHONE || UNITY_IOS
					if (_metrica == null && Application.platform == RuntimePlatform.IPhonePlayer) {
						_metrica = new YandexAppMetricaIOS();
					}
#elif UNITY_ANDROID
					if (_metrica == null && Application.platform == RuntimePlatform.Android) {
						_metrica = new YandexAppMetricaAndroid();
					}
#endif
					if (_metrica == null) {
						_metrica = new YandexAppMetricaDummy();
					}
				}
			}
			return _metrica;
		}
	}

	void SetupMetrica ()
	{
		var configuration = new YandexAppMetricaConfig(APIKey) { 
			SessionTimeout = (int)SessionTimeoutSec,
			LoggingEnabled = LoggingEnabled,
		};
			
#if !APP_METRICA_TRACK_LOCATION_DISABLED
		configuration.TrackLocationEnabled = TrackLocation;
		if (TrackLocation) {
			Input.location.Start ();
		}
#endif

		Instance.ActivateWithConfiguration(configuration);
	}

	private void Awake ()
	{
		if (!_isInitialized) {
			_isInitialized = true;
			DontDestroyOnLoad(this.gameObject);
			SetupMetrica();
		} else {
			Destroy(this.gameObject);
		}
	}

	private void Start ()
	{
		Instance.OnResumeApplication();
	}

	private void OnEnable ()
	{
		if (ExceptionsReporting) {
#if UNITY_5
			Application.logMessageReceived += HandleLog;
#else
			Application.RegisterLogCallback(HandleLog);
#endif
		}
	}
	
	private void OnDisable ()
	{
		if (ExceptionsReporting) {
#if UNITY_5
			Application.logMessageReceived -= HandleLog;
#else
			Application.RegisterLogCallback(null);
#endif
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if (_actualPauseStatus != pauseStatus) {
			_actualPauseStatus = pauseStatus;
			if (pauseStatus) {
				Instance.OnPauseApplication();
			} else {
				Instance.OnResumeApplication();
			}
		}
	}
	
	void Update()
	{
		if (ExceptionsReporting) {
			if (_handledLogEvents.Count > 0) {
				var eventsToReport = (ArrayList)_handledLogEvents.Clone();
				foreach (LogEvent handledLog in eventsToReport) {
					Instance.ReportError(handledLog.Condition, handledLog.StackTrace);
					_handledLogEvents.Remove(handledLog);
				}
			}

			var reports = CrashReport.reports;
			foreach (var report in reports) {
				Instance.ReportError(report.text, string.Format("Time: {0}", report.time));
				report.Remove();
			}
		}
	}
	
	private void HandleLog(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Exception) {
			_handledLogEvents.Add(new LogEvent{ Condition = condition, StackTrace = stackTrace });
		}
	}

}
