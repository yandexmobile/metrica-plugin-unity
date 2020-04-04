/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;

public class YandexAppMetricaNumberAttribute
{
    private const string AttributeName = "customNumber";

    private readonly string Key;

    public YandexAppMetricaNumberAttribute (string key)
    {
        Key = key;
    }

    public YandexAppMetricaUserProfileUpdate WithValue (double value)
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withValue", Key, value);
    }

    public YandexAppMetricaUserProfileUpdate WithValueIfUndefined (double value)
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withValueIfUndefined", Key, value);
    }

    public YandexAppMetricaUserProfileUpdate WithValueReset ()
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withValueReset", Key);
    }
}
