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

void ymm_activateWithConfigurationJSON(char *configurationJSON);
bool ymm_isAppMetricaActivated();

void ymm_reportEvent(char *message);
void ymm_reportEventWithParameters(char *message, char *parameters);
void ymm_reportError(char *condition, char *stackTrace);

void ymm_setLocationTracking(bool enabled);
void ymm_setLocation(double latitude, double longitude);
void ymm_resetLocation();

void ymm_setStatisticsSending(bool enabled);

void ymm_sendEventsBuffer();

void ymm_requestAppMetricaDeviceID(YMMRequestDeviceIDCallbackDelegate callbackDelegate, YMMAction actionPtr);

void ymm_setUserProfileID(char *userProfileID);
void ymm_reportUsertProfileJSON(char *userProfileJSON);

void ymm_reportRevenueJSON(char *revenueJSON);

char *ymm_getLibraryVersion();
