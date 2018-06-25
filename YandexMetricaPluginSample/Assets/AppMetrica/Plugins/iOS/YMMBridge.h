/*
 * Version for Unity
 * © 2015-2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import <Foundation/Foundation.h>

void ymm_activateWithConfigurationJSON(char *configurationJSON);
bool ymm_isAppMetricaActivated();

void ymm_reportEvent(char *message);
void ymm_reportEventWithParameters(char *message, char *parameters);
void ymm_reportError(char *condition, char *stackTrace);

void ymm_setLocationTracking(bool enabled);

void ymm_setLocation(double latitude, double longitude);

char * ymm_getLibraryVersion();

void ymm_setUserProfileID(char *userProfileID);
void ymm_reportUsertProfileJSON(char *userProfileJSON);

void ymm_reportRevenueJSON(char *revenueJSON);
