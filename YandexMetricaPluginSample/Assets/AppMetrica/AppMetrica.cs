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
	private uint MaxReportsCount = 10;
	
	[SerializeField]
	private uint DispatchPeriodSec = 90;
	
	[SerializeField]
	private uint SessionTimeoutSec = 300;

	[SerializeField]
	private bool TrackLocation = true;
	
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
					if (_metrica == null) {
						if (Application.platform == RuntimePlatform.IPhonePlayer) {
#if UNITY_IPHONE || UNITY_IOS
							_metrica = new YandexAppMetricaIOS();
#endif
						} else if (Application.platform == RuntimePlatform.Android) {
#if UNITY_ANDROID
							_metrica = new YandexAppMetricaAndroid();
#endif
						} 

						if (_metrica == null) {
							_metrica = new YandexAppMetricaDummy();
						}
					}
				}
			}
			return _metrica;
		}
	}

	void setupMetrica ()
	{
		Instance.MaxReportsCount = MaxReportsCount;
		Instance.SessionTimeout = SessionTimeoutSec;
		Instance.DispatchPeriod = DispatchPeriodSec;
		Instance.TrackLocationEnabled = TrackLocation;
		Instance.ReportCrashesEnabled = ExceptionsReporting;

		if (TrackLocation) {
			Input.location.Start ();
		}
	}

	private void Awake ()
	{
		if (!_isInitialized) {
			_isInitialized = true;
			DontDestroyOnLoad(this.gameObject);
			Instance.StartWithAPIKey(APIKey);
		} else {
			Destroy(this.gameObject);
		}
	}

	private void Start ()
	{
		setupMetrica ();
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
		if (_handledLogEvents.Count > 0) {
			var eventsToReport = (ArrayList)_handledLogEvents.Clone();
			foreach (LogEvent handledLog in eventsToReport) {
				Instance.ReportError(handledLog.Condition, handledLog.StackTrace);
				_handledLogEvents.Remove(handledLog);
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
