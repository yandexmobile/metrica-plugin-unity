/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

public class YandexAppMetricaGenderAttribute
{
    public enum Gender
    {
        MALE,
        FEMALE,
        OTHER
    }

    private const string AttributeName = "gender";

    public YandexAppMetricaUserProfileUpdate WithValue(Gender value)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withValue", null, value.ToString());
    }

    public YandexAppMetricaUserProfileUpdate WithValueReset()
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withValueReset", null);
    }
}
