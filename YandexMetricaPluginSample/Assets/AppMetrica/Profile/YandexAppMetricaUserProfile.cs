/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System.Collections.Generic;

public class YandexAppMetricaUserProfile
{
    private readonly List<YandexAppMetricaUserProfileUpdate> _updates = new List<YandexAppMetricaUserProfileUpdate>();

    public List<YandexAppMetricaUserProfileUpdate> GetUserProfileUpdates()
    {
        return new List<YandexAppMetricaUserProfileUpdate>(_updates);
    }

    public YandexAppMetricaUserProfile Apply(YandexAppMetricaUserProfileUpdate update)
    {
        _updates.Add(update);
        return this;
    }

    public YandexAppMetricaUserProfile ApplyFromArray(List<YandexAppMetricaUserProfileUpdate> updates)
    {
        _updates.AddRange(updates);
        return this;
    }
}
