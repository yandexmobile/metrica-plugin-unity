/*
 * Version for Unity
 * © 2022 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

/// <summary>
/// <para>The class to store Ad Revenue data.</para>
/// <para>The Ad Revenue object should be passed to the AppMetrica by using the
/// <see cref="IYandexAppMetrica.ReportAdRevenue(YandexAppMetricaAdRevenue)"/> method.</para>
/// </summary>
[Serializable]
public struct YandexAppMetricaAdRevenue
{
    /// <summary>
    /// Enum containing possible Ad Type values.
    /// </summary>
    /// <seealso cref="YandexAppMetricaAdRevenue.AdType"/>
    public enum AdTypeEnum
    {
        Native, Banner, Rewarded, Interstitial, Mrec, Other
    }

    /// <value>
    /// <para>Amount of money received via ad revenue.</para>
    /// It cannot be negative.
    /// </value>
    public decimal AdRevenue { get; private set; }

    /// <value>
    /// Currency in which money from <c>AdRevenue</c> is represented.
    /// </value>
    [NotNull]
    public string Currency { get; private set; }

    /// <value>
    /// <para>Ad type.</para>
    /// See possible values in <see cref="AdTypeEnum"/>.
    /// </value>
    public AdTypeEnum? AdType { get; set; }

    /// <value>
    /// <para>Ad network.</para>
    /// Maximum length is 100 symbols. If the value exceeds this limit it will be truncated by AppMetrica.
    /// </value>
    [CanBeNull]
    public string AdNetwork { get; set; }

    /// <value>
    /// <para>Id of ad unit.</para>
    /// Maximum length is 100 symbols. If the value exceeds this limit it will be truncated by AppMetrica.
    /// </value>
    [CanBeNull]
    public string AdUnitId { get; set; }

    /// <value>
    /// <para>Name of ad unit.</para>
    /// Maximum length is 100 symbols. If the value exceeds this limit it will be truncated by AppMetrica.
    /// </value>
    [CanBeNull]
    public string AdUnitName { get; set; }

    /// <value>
    /// <para>Id of ad placement.</para>
    /// Maximum length is 100 symbols. If the value exceeds this limit it will be truncated by AppMetrica.
    /// </value>
    [CanBeNull]
    public string AdPlacementId { get; set; }

    /// <value>
    /// <para>Name of ad placement.</para>
    /// Maximum length is 100 symbols. If the value exceeds this limit it will be truncated by AppMetrica.
    /// </value>
    [CanBeNull]
    public string AdPlacementName { get; set; }

    /// <value>
    /// <para>Precision.</para>
    /// Maximum length is 100 symbols. If the value exceeds this limit it will be truncated by AppMetrica.
    /// <para>Example: "publisher_defined", "estimated".</para>
    /// </value>
    [CanBeNull]
    public string Precision { get; set; }

    /// <value>
    /// <para>Arbitrary payload: additional info represented as key-value pairs.</para>
    /// Maximum size is 30 KB. If the value exceeds this limit it will be truncated by AppMetrica.
    /// </value>
    [CanBeNull]
    public IDictionary<string, string> Payload { get; set; }

    /// <summary>
    /// Creates the new instance of YandexAppMetricaAdRevenue.
    /// </summary>
    /// <param name="adRevenueMicros">Amount of money received via ad revenue represented as micros
    ///                               (actual value multiplied by 10^6).
    ///                               It will be converted to decimal.</param>
    /// <param name="currency">Currency.</param>
    public YandexAppMetricaAdRevenue(long adRevenueMicros, [NotNull] string currency) :
        this(Convert.ToDecimal(adRevenueMicros) / 1_000_000m, currency) {}

    /// <summary>
    /// Creates the new instance of YandexAppMetricaAdRevenue.
    /// </summary>
    /// <param name="adRevenue">Amount of money received via ad revenue represented as double.
    ///                         It will be converted to decimal.</param>
    /// <param name="currency">Currency.</param>
    public YandexAppMetricaAdRevenue(double adRevenue, [NotNull] string currency) :
        this(Convert.ToDecimal(GetFiniteDoubleOrDefault(adRevenue, 0)), currency) {}

    /// <summary>
    /// Creates the new instance of YandexAppMetricaAdRevenue.
    /// </summary>
    /// <param name="adRevenue">Amount of money received via ad revenue.</param>
    /// <param name="currency">Currency.</param>
    public YandexAppMetricaAdRevenue(decimal adRevenue, [NotNull] string currency) : this()
    {
        AdRevenue = adRevenue;
        Currency = currency;
        AdType = null;
        AdNetwork = null;
        AdUnitId = null;
        AdUnitName = null;
        AdPlacementId = null;
        AdPlacementName = null;
        Precision = null;
        Payload = null;
    }

    private static double GetFiniteDoubleOrDefault(double value, double fallback)
    {
        return double.IsNaN(value) || double.IsInfinity(value) ? fallback : value;
    }
}
