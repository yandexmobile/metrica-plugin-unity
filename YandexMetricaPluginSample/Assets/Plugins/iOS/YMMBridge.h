#import <Foundation/Foundation.h>

void ymm_startWithAPIKey(char *apiKey);

void ymm_reportEvent(char *message);
void ymm_reportEventWithParameters(char *message, char *parameters);
void ymm_reportError(char *condition, char *stackTrace);

void ymm_sendEventsBuffer();
void ymm_startNewSessionManually();

void ymm_setTrackLocationEnabled(bool enabled);
bool ymm_isTrackLocationEnabled();

void ymm_setLocation(double latitude, double longitude);

void ymm_setDispatchPeriod(unsigned int dispatchPeriodSeconds);
unsigned int ymm_dispatchPeriod();

void ymm_setMaxReportsCount(unsigned int maxReportsCount);
unsigned int ymm_maxReportsCount();

void ymm_setSessionTimeout(unsigned int sessionTimeoutSeconds);
unsigned int ymm_sessionTimeout();

void ymm_setReportsEnabled(bool enabled);
bool ymm_isReportsEnabled();

void ymm_setReportCrashesEnabled(bool enabled);
bool ymm_isReportCrashesEnabled();

void ymm_setCustomAppVersion(char *appVersion);

void ymm_setLogLevel(unsigned int level);

void ymm_setEnvironmentValue(char *key, char *value);

char * ymm_getLibraryVersion();
