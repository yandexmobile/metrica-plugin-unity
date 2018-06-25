﻿/*
 * Version for Unity
 * © 2015-2018 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;

public class YandexAppMetricaGenderAttribute
{
    private const string AttributeName = "gender";

    public enum Gender
    {
        MALE,
        FEMALE,
        OTHER
    }

    public YandexAppMetricaUserProfileUpdate WithValue (Gender value)
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withValue", null, value.ToString ());
    }

    public YandexAppMetricaUserProfileUpdate WithValueReset ()
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withValueReset", null);
    }
}
