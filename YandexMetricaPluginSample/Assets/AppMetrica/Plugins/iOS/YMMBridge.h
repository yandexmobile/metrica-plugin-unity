/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

// DO NOT REMOVE!!! It is required for AppMetrica Push Unity plugin.

#import <Foundation/Foundation.h>

typedef const void *YMMAction;

typedef void (*YMMRequestDeviceIDCallbackDelegate)(YMMAction action, const char *deviceId, const char *errorString);
typedef void (*YMMRequestTrackingAuthorization)(YMMAction action, const int status);

void ymm_activateWithConfigurationJSON(char *configurationJSON);
bool ymm_isAppMetricaActivated();

void ymm_resumeSession();
void ymm_pauseSession();

void ymm_reportAdRevenueJSON(char *adRevenueJson);
void ymm_reportEvent(char *message);
void ymm_reportEventWithParameters(char *message, char *parameters);
void ymm_reportError(char *condition, char *stackTrace);
void ymm_reportErrorWithIdentifier(char *groupIdentifier, char *condition, char *stackTrace);
void ymm_reportErrorWithException(char *groupIdentifier, char *condition, char *exceptionJson);
void ymm_reportUnhandledException(char *errorJson);
void ymm_reportErrorWithMessage(char *errorJson, char *message);
void ymm_reportErrorWithIdentifierAndMessage(char *groupIdentifier, char *message, char *errorJson);

void ymm_setLocationTracking(bool enabled);
void ymm_setLocation(double latitude, double longitude);
void ymm_resetLocation();

void ymm_setStatisticsSending(bool enabled);

void ymm_sendEventsBuffer();

void ymm_requestAppMetricaDeviceID(YMMRequestDeviceIDCallbackDelegate callbackDelegate, YMMAction actionPtr);

void ymm_setUserProfileID(char *userProfileID);
void ymm_reportUsertProfileJSON(char *userProfileJSON);

void ymm_reportRevenueJSON(char *revenueJSON);

void ymm_reportReferralUrl(char *referralUrl);
void ymm_reportAppOpen(char *deeplink);

char *ymm_getLibraryVersion();

void ymm_putErrorEnvironmentValue(char *key, char *value);

void ymm_requestTrackingAuthorization(YMMRequestTrackingAuthorization callbackDelegate, YMMAction actionPtr);
