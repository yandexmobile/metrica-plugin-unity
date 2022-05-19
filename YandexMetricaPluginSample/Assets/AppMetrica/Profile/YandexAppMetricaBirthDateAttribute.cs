/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;

public class YandexAppMetricaBirthDateAttribute
{
    private const string AttributeName = "birthDate";

    public YandexAppMetricaUserProfileUpdate WithAge(int age)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withAge", null, age);
    }

    public YandexAppMetricaUserProfileUpdate WithBirthDate(DateTime date)
    {
        return WithBirthDate(date.Year, date.Month, date.Day);
    }

    public YandexAppMetricaUserProfileUpdate WithBirthDate(int year)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withBirthDate", null, year);
    }

    public YandexAppMetricaUserProfileUpdate WithBirthDate(int year, int month)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withBirthDate", null, year, month);
    }

    public YandexAppMetricaUserProfileUpdate WithBirthDate(int year, int month, int day)
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withBirthDate", null, year, month, day);
    }

    public YandexAppMetricaUserProfileUpdate WithValueReset()
    {
        return new YandexAppMetricaUserProfileUpdate(AttributeName, "withValueReset", null);
    }
}
