/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;

public class YandexAppMetricaCounterAttribute
{
    private const string AttributeName = "customCounter";

    private readonly string Key;

    public YandexAppMetricaCounterAttribute (string key)
    {
        Key = key;
    }

    public YandexAppMetricaUserProfileUpdate WithDelta (double value)
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withDelta", Key, value);
    }
}
