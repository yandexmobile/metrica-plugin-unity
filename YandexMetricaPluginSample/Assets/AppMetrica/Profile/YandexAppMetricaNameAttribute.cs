﻿/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;

public class YandexAppMetricaNameAttribute
{
    private const string AttributeName = "name";

    public YandexAppMetricaUserProfileUpdate WithValue (string value)
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withValue", null, value);
    }

    public YandexAppMetricaUserProfileUpdate WithValueReset ()
    {
        return new YandexAppMetricaUserProfileUpdate (AttributeName, "withValueReset", null);
    }
}
