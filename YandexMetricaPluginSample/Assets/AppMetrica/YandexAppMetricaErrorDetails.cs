using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

public class YandexAppMetricaErrorDetails
{
    public YandexAppMetricaErrorDetails()
    {
        Platform = "unity";
        VirtualMachineVersion = Environment.Version.ToString();
    }

    [CanBeNull] public string ExceptionClass { get; set; }

    [CanBeNull] public string Message { get; set; }

    [CanBeNull] public List<YandexAppMetricaStackTraceItem> Stacktrace { get; set; }

    [NotNull] public string Platform { get; private set; }

    [NotNull] public string VirtualMachineVersion { get; private set; }

    [CanBeNull] public Dictionary<string, string> PluginEnvironment { get; set; }

    [NotNull]
    public static YandexAppMetricaErrorDetails From([NotNull] Exception exception)
    {
        // there is no java stack trace and sometimes file names in the stack frame, so we parse the string stack trace
        List<YandexAppMetricaStackTraceItem> stacktrace = exception.StackTrace
            .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
            .Where(it =>
            {
                string trimStr = it.Trim();
                return trimStr.Length != 0 || !trimStr.StartsWith("Rethrow as"); // for inner exception
            })
            .SkipWhile(it => it == exception.Message) // for android crash first item is exception class + message
            .Select(YandexAppMetricaStackTraceItem.From)
            .ToList();

        Dictionary<string, string> env = new Dictionary<string, string>
        {
            { "Unity", Application.unityVersion }, { "Source", exception.Source }
        };
        if (exception.HelpLink != null)
        {
            env.Add("HelpLink", exception.HelpLink);
        }

        foreach (DictionaryEntry entry in exception.Data)
        {
            env.Add(entry.Key.ToString(), entry.Value.ToString());
        }

        return new YandexAppMetricaErrorDetails
        {
            ExceptionClass = exception.GetType().FullName,
            Message = exception.Message,
            Stacktrace = stacktrace,
            PluginEnvironment = env
        };
    }

    public static YandexAppMetricaErrorDetails FromLogCallback(
        [NotNull] string condition,
        [CanBeNull] string stacktraceFromLog)
    {
        string[] conditionParts = condition.Split(new[] { ":" }, 2, StringSplitOptions.None);
        string exceptionClass = conditionParts.Length == 2 ? conditionParts[0].Trim() : "Exception";
        string message = (conditionParts.Length == 2 ? conditionParts[1] : conditionParts[0]).Trim();

        List<YandexAppMetricaStackTraceItem> stacktrace = stacktraceFromLog != null
            ? stacktraceFromLog.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Where(it =>
                {
                    string trimStr = it.Trim();
                    return trimStr.Length != 0 && !trimStr.StartsWith("Rethrow as"); // for inner exception
                })
                // for android crash first item is exception class + message
                .SkipWhile(it => "AndroidJavaException: " + it == condition)
                .Select(YandexAppMetricaStackTraceItem.From)
                .ToList()
            : null;

        return new YandexAppMetricaErrorDetails
        {
            ExceptionClass = exceptionClass,
            Message = message,
            Stacktrace = stacktrace,
            PluginEnvironment = new Dictionary<string, string>
            {
                { "Unity", Application.unityVersion }, { "Source", "From LogCallback" }
            }
        };
    }

    public override string ToString()
    {
        string pluginEnvironment = null;
        if (PluginEnvironment != null)
        {
            pluginEnvironment = string.Join(", ",
                PluginEnvironment.Select(it => string.Format("{0} => {1}", it.Key, it.Value)).ToArray());
        }

        string stacktrace = null;
        if (Stacktrace != null)
        {
            stacktrace = string.Join("\n  ", Stacktrace.Select(it => it.ToString()).ToArray());
        }

        return "Class: " + ExceptionClass + "\n" +
               "Message: " + Message + "\n" +
               "Platform: " + Platform + "\n" +
               "VirtualMachineVersion: " + VirtualMachineVersion + "\n" +
               "PluginEnvironment: [" + pluginEnvironment + "]\n" +
               "Stacktrace:\n  " + stacktrace;
    }
}

// UnityEngine.AndroidJNISafe.CheckException () (at /Users/bokken/buildslave/unity/build/Modules/AndroidJNI/AndroidJNISafe.cs:24)
// at UnityEngine.AndroidJNISafe.CheckException () [0x0008d] in /Users/bokken/buildslave/unity/build/Modules/AndroidJNI/AndroidJNISafe.cs:24
// CrashSceneManager:<ReportErrorMethodsGUI>b__10_5() (at /sample/Assets/AppMetricaSample/CrashSceneManager.cs:168)
// CrashSceneManager.CSharpCrashGUI () (at <5592b27588074c6b8f13ce7f73fdcb59>:0)
// at CrashSceneManager+<>c.<ReportErrorMethodsGUI>b__10_0 () [0x00000] in <7992989e29fa41c989068b22d7ae468e>:0
// CrashHelper.crash(CrashHelper.java:8)
// com.unity3d.player.UnityPlayer.nativeRender(Native Method)
// com.unity3d.player.UnityPlayer.access$300(Unknown Source:0)
// UnityEngine.AndroidJavaObject:CallStatic(String, Object[])
public class YandexAppMetricaStackTraceItem
{
    private const string StacktraceItemRegexp = @"(?<class>[^\s()]+)[.:](?<method>[^\s\.()]+)\s?(?<params>\(.*\))?";

    private const string StacktraceItemWithFileRegexp =
        StacktraceItemRegexp + @".*(/|\||\\|at |in |\()(?<file>[^:)]+):(?<line>\d+)";

    [CanBeNull] public string ClassName { get; set; }

    [CanBeNull] public string MethodName { get; set; }

    [CanBeNull] public string FileName { get; set; }

    public int? Line { get; set; }

    public int? Column { get; set; }

    [NotNull]
    public static YandexAppMetricaStackTraceItem From([NotNull] string stackTraceLine)
    {
        stackTraceLine = stackTraceLine.Trim();
        // stacktrace with file info
        Match match = Regex.Match(stackTraceLine, StacktraceItemWithFileRegexp);
        if (!match.Success)
        {
            // stacktrace without file info
            match = Regex.Match(stackTraceLine, StacktraceItemRegexp);
            if (!match.Success)
            {
                throw new FormatException(string.Format("Failed to parse stacktrace element '{0}'", stackTraceLine));
            }
        }

        string lineStr = GetGroupValueOrNull(match, "line");
        return new YandexAppMetricaStackTraceItem
        {
            ClassName = GetGroupValueOrNull(match, "class"),
            MethodName = GetGroupValueOrNull(match, "method"),
            FileName = GetGroupValueOrNull(match, "file"),
            Line = lineStr == null ? (int?)null : int.Parse(lineStr),
            Column = null
        };
    }

    [CanBeNull]
    private static string GetGroupValueOrNull([NotNull] Match match, [NotNull] string groupName)
    {
        return match.Groups[groupName].Success ? match.Groups[groupName].Value : null;
    }

    public override string ToString()
    {
        return string.Format("{0}.{1} (at {2}:{3}:{4})", ClassName, MethodName, FileName, Line ?? -1, Column ?? -1);
    }
}
