/*
 * Version for Unity
 * © 2015-2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import <Foundation/Foundation.h>

void ymm_activateWithAPIKey(char *apiKey);
void ymm_activateWithConfigurationJSON(char *configurationJSON);
bool ymm_isAppMetricaActivated();

void ymm_reportEvent(char *message);
void ymm_reportEventWithParameters(char *message, char *parameters);
void ymm_reportError(char *condition, char *stackTrace);

void ymm_setTrackLocationEnabled(bool enabled);

void ymm_setLocation(double latitude, double longitude);

void ymm_setSessionTimeout(unsigned int sessionTimeoutSeconds);

void ymm_setReportCrashesEnabled(bool enabled);

void ymm_setCustomAppVersion(char *appVersion);

void ymm_setLoggingEnabled(bool enabled);

void ymm_setEnvironmentValue(char *key, char *value);

char * ymm_getLibraryVersion();
