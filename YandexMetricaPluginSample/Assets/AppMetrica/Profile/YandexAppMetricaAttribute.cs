/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

public abstract class YandexAppMetricaAttribute
{
    public static YandexAppMetricaBirthDateAttribute BirthDate()
    {
        return new YandexAppMetricaBirthDateAttribute();
    }

    public static YandexAppMetricaGenderAttribute Gender()
    {
        return new YandexAppMetricaGenderAttribute();
    }

    public static YandexAppMetricaNameAttribute Name()
    {
        return new YandexAppMetricaNameAttribute();
    }

    public static YandexAppMetricaNotificationsEnabledAttribute NotificationsEnabled()
    {
        return new YandexAppMetricaNotificationsEnabledAttribute();
    }

    public static YandexAppMetricaBooleanAttribute CustomBoolean(string key)
    {
        return new YandexAppMetricaBooleanAttribute(key);
    }

    public static YandexAppMetricaCounterAttribute CustomCounter(string key)
    {
        return new YandexAppMetricaCounterAttribute(key);
    }

    public static YandexAppMetricaNumberAttribute CustomNumber(string key)
    {
        return new YandexAppMetricaNumberAttribute(key);
    }

    public static YandexAppMetricaStringAttribute CustomString(string key)
    {
        return new YandexAppMetricaStringAttribute(key);
    }
}
