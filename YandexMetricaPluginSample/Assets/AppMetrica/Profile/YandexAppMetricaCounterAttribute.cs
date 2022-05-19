/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

public class YandexAppMetricaCounterAttribute
{
    private const string AttributeName = "customCounter";

    private readonly string _key;

    public YandexAppMetricaCounterAttribute(string key)
    {
        _key = key;
    }

    public YandexAppMetricaUserProfileUpdate WithDelta(double value)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withDelta", _key, value);
    }
}
