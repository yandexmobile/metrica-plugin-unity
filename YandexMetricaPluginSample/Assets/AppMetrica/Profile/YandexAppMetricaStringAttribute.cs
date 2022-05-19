/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

public class YandexAppMetricaStringAttribute
{
    private const string AttributeName = "customString";

    private readonly string _key;

    public YandexAppMetricaStringAttribute(string key)
    {
        _key = key;
    }

    public YandexAppMetricaUserProfileUpdate WithValue(string value)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withValue", _key, value);
    }

    public YandexAppMetricaUserProfileUpdate WithValueIfUndefined(string value)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withValueIfUndefined", _key, value);
    }

    public YandexAppMetricaUserProfileUpdate WithValueReset()
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withValueReset", _key);
    }
}
