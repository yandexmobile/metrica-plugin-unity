/*
 * Version for Unity
 * © 2015-2022 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using YMMJSONUtils;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class CrashSceneManager : BaseSceneManager
{
    private readonly Dictionary<string, object> _errorEnvironment = new Dictionary<string, object>();

#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
    private AndroidJavaObject _crashHelper;
#endif
    private string _key = "key";
    private string _value = "value";

    private void Awake()
    {
#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
        _crashHelper = new AndroidJavaClass("CrashHelper");
#endif
    }

    protected override void Content()
    {
        CSharpCrashGUI();
        JavaCrashGUI();
        ObjectiveCCrashGUI();
        ReportErrorMethodsGUI();
        ParamsGUI();
    }

    protected override void BottomContent()
    {
        LoadSceneButton("Back To Main Scene", "MainScene");
    }

    private void CSharpCrashGUI()
    {
        GUILayout.Label("C# crash");
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Crash", () =>
        {
            ThrowCrash("unity crash");
        });
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Crash in other line", () =>
        {
            throw new SystemException("unity crash");
        });
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Crash with other message", () =>
        {
            ThrowCrash("yet another unity crash");
        });
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Other crash", () => ((GameObject)null).SendMessage(""));
    }

    private void JavaCrashGUI()
    {
#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
        GUILayout.Label("Java crash");
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Crash", () => _crashHelper.CallStatic("crash", "java crash"));
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Crash in other line", () => _crashHelper.CallStatic("crashInOtherLine", "java crash"));
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Crash with other message", () => _crashHelper.CallStatic("crash", "yet another java crash"));
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Other crash", () => _crashHelper.CallStatic("otherCrash", "other java crash"));
        // Sent as crash. Caught by the native SDK.
        Button("Crash in Java code in other Thread",
            () => _crashHelper.CallStatic("crashInOtherThread", "java crash in other thread"));
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Java -> C# Crash", () =>
        {
            _crashHelper.CallStatic("callCs", new CrashHelperCallbackJava(() =>
            {
                ThrowCrash("java -> c# crash");
            }));
        });
        // Sent as error. Caught in `AppMetrica.HandleLog`.
        Button("Java -> C# -> Java Crash", () =>
        {
            _crashHelper.CallStatic("callCs", new CrashHelperCallbackJava(() =>
            {
                _crashHelper.CallStatic("crash", "java -> c# -> java crash");
            }));
        });
#endif
    }

    // Sent as crash. Caught by the native SDK.
    private void ObjectiveCCrashGUI()
    {
#if UNITY_IOS
        GUILayout.Label("Objective-C crash");
        Button("Crash", () => ymm_sample_crash("Objective-C crash"));
        Button("Crash in other line", () => ymm_sample_crashInOtherLine("Objective-C crash"));
        Button("Crash with other message", () => ymm_sample_crash("other Objective-C crash message"));
        Button("Other crash", () => ymm_sample_otherCrash("other Objective-C crash"));
#endif
    }

    private void ReportErrorMethodsGUI()
    {
        GUILayout.Label("ReportError methods");
        Button("ReportUnhandledException", () =>
        {
            try
            {
                ThrowCrash("try/catch unhandled exception");
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportUnhandledException(e);
            }
        });
        Button("ReportError", () =>
        {
            try
            {
                ThrowCrash("try/catch error");
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportError(e, "try/catch error message");
            }
        });
        Button("ReportError in other line", () =>
        {
            try
            {
                throw new SystemException("try/catch error");
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportError(e, "try/catch error message");
            }
        });
        Button("ReportError with identifier", () =>
        {
            try
            {
                ThrowCrash("try/catch error with identifier");
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportError("error group", "try/catch error message", e);
            }
        });
#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
        Button("Java Error", () =>
        {
            try
            {
                _crashHelper.CallStatic("crash", "catch java crash in unity");
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportError(e, "try/catch error message");
            }
        });
        Button("Java -> C# Error", () =>
        {
            try
            {
                _crashHelper.CallStatic("callCs", new CrashHelperCallbackJava(() =>
                {
                    ThrowCrash("catch java -> c# crash");
                }));
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportError(e, "try/catch java -> c# error message");
            }
        });
        Button("Java -> C# -> Java Error", () =>
        {
            try
            {
                _crashHelper.CallStatic("callCs", new CrashHelperCallbackJava(() =>
                {
                    _crashHelper.CallStatic("crash", "catch java -> c# -> java crash");
                }));
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportError(e, "try/catch java -> c# -> java error message");
            }
        });
#endif
#if UNITY_IOS
        // Sent as crash, because it cannot be caught from C#. Caught by the native SDK.
        Button("Objective-C Error", () =>
        {
            try
            {
                ymm_sample_crash("Objective-C crash");
            }
            catch (Exception e)
            {
                AppMetrica.Instance.ReportError("Objective-C error with C# Exception", "Condition: " + e.Message, e);
            }
        });
#endif
    }

    private void ParamsGUI()
    {
        _key = GUILayout.TextField(_key);
        _value = GUILayout.TextField(_value);
        Button("Add error environment", () =>
        {
            _errorEnvironment[_key] = _value;
            AppMetrica.Instance.PutErrorEnvironmentValue(_key, _value);
        });
        Button("Clear error environment", () =>
        {
            foreach (string key in _errorEnvironment.Keys)
            {
                AppMetrica.Instance.PutErrorEnvironmentValue(key, null);
            }

            _errorEnvironment.Clear();
        });
        GUILayout.Label(JSONEncoder.Encode(_errorEnvironment));
    }

    private static void ThrowCrash(string message)
    {
        Debug.Log("crash with message " + message);
        throw new SystemException(message);
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void ymm_sample_crash(string message);

    [DllImport("__Internal")]
    private static extern void ymm_sample_crashInOtherLine(string message);

    [DllImport("__Internal")]
    private static extern void ymm_sample_otherCrash(string message);
#endif

#if UNITY_2018_2_OR_NEWER && UNITY_ANDROID
    private class CrashHelperCallbackJava : AndroidJavaProxy
    {
        private const string AndroidClassName = "CrashHelperCallback";

        private readonly Action _action;

        public CrashHelperCallbackJava(Action action) : base(AndroidClassName)
        {
            _action = action;
        }

        // ReSharper disable once InconsistentNaming
        public void invoke()
        {
            _action();
        }
    }
#endif
}
