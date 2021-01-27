using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashSceneManager : MonoBehaviour
{
    private const string DEFAULT_KEY = "key";
    private const string DEFAULT_VALUE = "value";
    
    private Dictionary<string, object> errorEnvironment = new Dictionary<string, object> ();
    private string key = DEFAULT_KEY;
    private string value = DEFAULT_VALUE;
    private Vector2 _scrollPosition;
    
#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
    private AndroidJavaObject _crashHelper;
#endif
    
#if UNITY_IOS
    [DllImport ("__Internal")]
    private static extern void ymm_sample_crash (string message);
    
    [DllImport ("__Internal")]
    private static extern void ymm_sample_crashInOtherLine (string message);
    
    [DllImport ("__Internal")]
    private static extern void ymm_sample_otherCrash (string message);
#endif

    private void Awake()
    {
#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
    _crashHelper = new AndroidJavaClass ("CrashHelper");
#endif
    }
    
    private void InitGUI ()
    {
        GUI.skin.button.fontSize = 40;
        GUI.skin.textField.fontSize = 35;
        GUI.contentColor = Color.white;
        GUI.skin.label.fontSize = 40;
    }
    
    private void OnGUI ()
    {
        InitGUI ();
        
        _scrollPosition = GUILayout.BeginScrollView (_scrollPosition);
        CSharpCrashGUI ();
        JavaCrashGUI ();
        ObjectiveCCrashGUI ();
        CSharpErrorGUI ();
        ParamsGUI ();
        GUILayout.EndScrollView ();
        
        if (Button ("Back To Main Scene")) {
            SceneManager.LoadScene ("MainScene");
        }
    }
    
    private void ThrowCSharpCrash (string msg)
    {
        throw new SystemException (msg);
    }

    // There will be the following groups of errors (names of the buttons are specified):
    // - Crash + Crash in other line
    // - Crash with other message
    // - Other crash
    private void CSharpCrashGUI ()
    {
        Label ("C# crash");
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "${exception class name}: ${message}"
        if (Button ("Crash")) {
            ThrowCSharpCrash ("crash from unity");
        }
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "${exception class name}: ${message}"
        if (Button ("Crash in other line")) {
            throw new SystemException ("crash from unity");
        }
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "${exception class name}: ${message}"
        if (Button ("Crash with other message")) {
            ThrowCSharpCrash ("other crash message from unity");
        }
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "${exception class name}: ${message}"
        if (Button ("Other crash")) {
            GameObject nullGameObject = null;
            nullGameObject.SendMessage ("");
        }
    }

    // There will be the following groups of errors (names of the buttons are specified):
    // - Crash + Crash in other line
    // - Crash with other message
    // - Other crash
    private void JavaCrashGUI ()
    {
#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
        Label ("Java crash");
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "AndroidJavaException: ${java exception class name}: ${message}"
        if (Button ("Crash")) {
            _crashHelper.CallStatic ("crash", "java crash");
        }
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "AndroidJavaException: ${java exception class name}: ${message}"
        if (Button ("Crash in other line")) {
            _crashHelper.CallStatic ("crashInOtherLine", "java crash");
        }
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "AndroidJavaException: ${java exception class name}: ${message}"
        if (Button ("Crash with other message")) {
            _crashHelper.CallStatic ("crash", "other java crash message");
        }
        // Sent as an error. Caught in `AppMetrica.HandleLog`.
        // groupIdentifier = "AndroidJavaException: ${java exception class name}: ${message}"
        if (Button ("Other crash")) {
            _crashHelper.CallStatic ("otherCrash", "other java crash");
        }
        // Sent as a crash. Caught by the native SDK.
        if (Button ("Crash in Java code in other Thread")) {
            _crashHelper.CallStatic ("crashInOtherThread", "Java crash in other thread");
        }
#endif
    }

    // Sent as a crash. Caught by the native SDK.
    private void ObjectiveCCrashGUI ()
    {
#if UNITY_IOS
        Label ("Objective-C crash");
        if (Button ("Crash")) {
            ymm_sample_crash ("Objective-C crash");
        }
        
        if (Button ("Crash in other line")) {
            ymm_sample_crashInOtherLine ("Objective-C crash");
        }
        
        if (Button ("Crash with other message")) {
            ymm_sample_crash ("other Objective-C crash message");
        }
        
        if (Button ("Other crash")) {
            ymm_sample_otherCrash ("other Objective-C crash");
        }
#endif
    }

    // Each error will have its own group
    private void CSharpErrorGUI ()
    {
        Label ("Error");
        // Android: Always grouped into a group:
        //          (Native Method)
        //          com.unity3d.player.UnityPlayer.nativeRender
        // iOS:     Grouping by message
        if (Button ("Old error without stacktrace")) {
            AppMetrica.Instance.ReportError ("Old error without stacktrace", null);
        }
        // Android: Always grouped into a group:
        //          java.lang.Throwable at (Native Method)
        //          com.unity3d.player.UnityPlayer.nativeRender
        // iOS:     Grouping by message
        if (Button ("Old error")) {
            try {
                ThrowCSharpCrash ("error from unity");
            } catch (Exception e) {
                AppMetrica.Instance.ReportError ("Condition: " + e.Message, "StackTrace: " + e.StackTrace);
            }
        }
        // Grouping by groupIdentifier
        if (Button ("Error with only identifier")) {
            AppMetrica.Instance.ReportError ("Error with only identifier", null, (Exception) null);
        }
        // Grouping by groupIdentifier
        if (Button ("Error without exception")) {
            AppMetrica.Instance.ReportError ("error identifier without exception",
                "error condition without exception", (Exception) null);
        }
        // Grouping by groupIdentifier
        if (Button ("Error with string stacktrace")) {
            try {
                ThrowCSharpCrash ("error from unity");
            } catch (Exception e) {
                AppMetrica.Instance.ReportError ("Error with string stacktrace",
                    "Condition: " + e.Message, "StackTrace: " + e.StackTrace);
            }
        }
        // Grouping by groupIdentifier
        if (Button ("Error with C# Exception")) {
            try {
                ThrowCSharpCrash ("error from unity");
            } catch (Exception e) {
                AppMetrica.Instance.ReportError ("Error with C# Exception", "Condition: " + e.Message, e);
            }
        }
#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
        // Android: Grouping by groupIdentifier
        if (Button ("Java Error")) {
            try {
                _crashHelper.CallStatic ("crash", "java error");
            } catch (Exception e) {
                AppMetrica.Instance.ReportError ("Java error with C# Exception", "Condition: " + e.Message, e);
            }
        }
#endif
#if UNITY_IOS
        // It is sent as a crash, because it cannot be caught from C#. Caught by the native SDK.
        if (Button ("Objective-C Error")) {
            try {
                ymm_sample_crash ("Objective-C crash");
            } catch (Exception e) {
                AppMetrica.Instance.ReportError ("Objective-C error with C# Exception", "Condition: " + e.Message, e);
            }
        }
#endif
    }
    
    void ParamsGUI ()
    {
        key = GUILayout.TextField (key);
        value = GUILayout.TextField (value);
        if (Button ("Add error environment")) {
            errorEnvironment [key] = value;
            AppMetrica.Instance.PutErrorEnvironmentValue (key, value);
            
        }
        if (Button ("Clear error environment")) {
            foreach (var key in errorEnvironment.Keys) {
                AppMetrica.Instance.PutErrorEnvironmentValue (key, null);
            }
            errorEnvironment.Clear ();
        }
        Label (YMMJSONUtils.JSONEncoder.Encode (errorEnvironment));
    }

    private bool Button (string title)
    {
        return GUILayout.Button (title, GUILayout.Width (Screen.width - 10), GUILayout.Height (Screen.height / 13));
    }

    private void Label (string text)
    {
        GUILayout.Label (text);
    }
}
