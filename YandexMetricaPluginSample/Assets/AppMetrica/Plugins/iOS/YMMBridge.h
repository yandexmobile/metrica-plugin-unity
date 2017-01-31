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
