/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public delegate void ConfigUpdateHandler(YandexAppMetricaConfig config);

public interface IYandexAppMetrica
{
    /// <summary>
    ///     <para>Gets AppMetrica activation config with which it was activated.</para>
    /// </summary>
    /// <value>Activation config.</value>
    YandexAppMetricaConfig? ActivationConfig { get; }

    /// <summary>
    ///     <para>Gets the library version.</para>
    ///     <para>Android: public static String getLibraryVersion()</para>
    ///     <para>iOS: +(NSString *)libraryVersion</para>
    /// </summary>
    /// <value>The library version.</value>
    string LibraryVersion { get; }

    /// <summary>
    ///     <para>Gets the library API level.</para>
    ///     <para>Android: public static int getLibraryApiLevel()</para>
    ///     <para>iOS: Not implemented</para>
    /// </summary>
    /// <value>The library API level.</value>
    int LibraryApiLevel { get; }

    /// <summary>
    ///     Occurs on metrica activation.
    /// </summary>
    event ConfigUpdateHandler OnActivation;

    /// <summary>
    ///     Activates AppMetrica with configuration.
    /// </summary>
    /// <param name="config">AppMetrica configuration.</param>
    void ActivateWithConfiguration(YandexAppMetricaConfig config);

    /// <summary>
    ///     Track session resume.
    /// </summary>
    void ResumeSession();

    /// <summary>
    ///     Track session pause.
    /// </summary>
    void PauseSession();

    /// <summary>
    /// Sends information about ad revenue.
    /// </summary>
    /// <param name="adRevenue">Object containing the information about ad revenue.</param>
    /// <seealso cref="YandexAppMetricaAdRevenue"/>
    void ReportAdRevenue(YandexAppMetricaAdRevenue adRevenue);

    /// <summary>
    ///     <para>Reports the event.</para>
    ///     <para>Android: public static void reportEvent(final String eventName)</para>
    ///     <para>iOS: +(void)reportEvent:(NSString *)message onFailure:(void (^)(NSError *))onFailure</para>
    /// </summary>
    /// <param name="message">Report message.</param>
    void ReportEvent(string message);

    /// <summary>
    ///     <para>Reports the event.</para>
    ///     <para>Android: public static void reportEvent(final String eventName, final Map&lt;String, Object&gt; attributes)</para>
    ///     <para>
    ///         iOS: +(void)reportEvent:(NSString *)message parameters:(NSDictionary *)params onFailure:(void (^)(NSError
    ///         *))onFailure
    ///     </para>
    /// </summary>
    /// <param name="message">Report message.</param>
    /// <param name="parameters">Custom parameters.</param>
    void ReportEvent(string message, IDictionary<string, object> parameters);

    void ReportEvent(string message, string json);

    /// <summary>
    ///     <para>Reports the error.</para>
    ///     <para>Android: public static void reportError(final String message, final Throwable error)</para>
    ///     <para>
    ///         iOS: +(void)reportError:(NSString *)message exception:(NSException *)exception onFailure:(void (^)(NSError
    ///         *))onFailure
    ///     </para>
    /// </summary>
    /// <param name="condition">Report message.</param>
    /// <param name="stackTrace">Exception stack trace.</param>
    [Obsolete("Use the ReportError(Exception exception, string condition) instead.")]
    void ReportError([NotNull] string condition, [CanBeNull] string stackTrace);

    [Obsolete("Use the ReportError(string groupIdentifier, string condition, Exception exception) instead.")]
    void ReportError([NotNull] string groupIdentifier, [CanBeNull] string condition, [CanBeNull] string stackTrace);

    void ReportError([NotNull] string groupIdentifier, [CanBeNull] string condition, [CanBeNull] Exception exception);

    void ReportError(
        [NotNull] string groupIdentifier,
        [CanBeNull] string condition,
        [CanBeNull] YandexAppMetricaErrorDetails errorDetails
    );

    void ReportError([NotNull] Exception exception, [CanBeNull] string condition);

    void ReportError([NotNull] YandexAppMetricaErrorDetails errorDetails, [CanBeNull] string condition);

    void ReportUnhandledException([NotNull] Exception exception);

    void ReportUnhandledException([NotNull] YandexAppMetricaErrorDetails errorDetails);

    void ReportErrorFromLogCallback([NotNull] string condition, [CanBeNull] string stackTrace);

    /// <summary>
    ///     <para>Sets the track location enabled.</para>
    ///     <para>Android: public static void setLocationTracking(final boolean enabled)</para>
    ///     <para>iOS: +(void)setLocationTracking:(BOOL)enabled</para>
    /// </summary>
    /// <param name="enabled">If set to <c>true</c> enabled.</param>
    void SetLocationTracking(bool enabled);

    /// <summary>
    ///     <para>Sets the location.</para>
    ///     <para>Android: public static void setLocation(final Location location)</para>
    ///     <para>iOS: +(void)setLocation:(CLLocation *)location</para>
    /// </summary>
    /// <param name="coordinates">Location coordinates(latitude and longitude).</param>
    void SetLocation(YandexAppMetricaConfig.Coordinates? coordinates);

    void SetUserProfileID(string userProfileID);

    void ReportUserProfile(YandexAppMetricaUserProfile userProfile);

    void ReportRevenue(YandexAppMetricaRevenue revenue);

    void SetStatisticsSending(bool enabled);

    void SendEventsBuffer();

    void RequestAppMetricaDeviceID(Action<string, YandexAppMetricaRequestDeviceIDError?> action);

    void ReportReferralUrl(string referralUrl);
    void ReportAppOpen(string deeplink);

    void PutErrorEnvironmentValue(string key, string value);

    void RequestTrackingAuthorization(Action<YandexAppMetricaRequestTrackingStatus> action);
}
