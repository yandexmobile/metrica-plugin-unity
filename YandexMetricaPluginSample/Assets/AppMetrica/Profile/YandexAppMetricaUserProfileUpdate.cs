/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

public struct YandexAppMetricaUserProfileUpdate
{
    public string AttributeName { get; set; }

    public string MethodName { get; set; }

    public string Key { get; set; }

    public object[] Values { get; set; }

    public YandexAppMetricaUserProfileUpdate(
        string attributeName,
        string methodName,
        string key,
        params object[] values) : this()
    {
        AttributeName = attributeName;
        MethodName = methodName;
        Key = key;
        Values = values;
    }
}
