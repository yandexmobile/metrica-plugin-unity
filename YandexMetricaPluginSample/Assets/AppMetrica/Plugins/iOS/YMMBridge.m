#import "YMMBridge.h"

#import <YandexMobileMetrica/YandexMobileMetrica.h>
#import <CoreLocation/CoreLocation.h>

static NSString *const kYMMUnityExceptionName = @"UnityException";

static bool *g_ymm_isAppMetricaActivated = false;

void ymm_activateWithAPIKey(char *apiKey)
{
    if (apiKey != NULL) {
        NSString *apiKeyString = [NSString stringWithUTF8String:apiKey];
        if (apiKeyString.length > 0) {
            [YMMYandexMetrica activateWithApiKey:apiKeyString];
            g_ymm_isAppMetricaActivated = true;
        }
        else {
            NSLog(@"AppMetrica could not be activated with an empty API key.");
        }
    }
}

YMMYandexMetricaConfiguration *ymm_configurationFromDictionary(NSDictionary *configDictionary)
{
    YMMYandexMetricaConfiguration *config =
    [[YMMYandexMetricaConfiguration alloc] initWithApiKey:configDictionary[@"ApiKey"]];

    if (configDictionary[@"AppVersion"] != nil) {
        config.customAppVersion = (NSString *)configDictionary[@"AppVersion"];
    }
    if (configDictionary[@"Location"] != nil) {
        NSDictionary *locationDictionary = configDictionary[@"Location"];
        CLLocationDegrees latitude = [locationDictionary[@"Latitude"] doubleValue],
        longitude = [locationDictionary[@"Longitude"] doubleValue];
        config.location = [[CLLocation alloc] initWithLatitude:latitude
                                                     longitude:longitude];
    }
    if (configDictionary[@"SessionTimeout"] != nil) {
        config.sessionTimeout = [configDictionary[@"SessionTimeout"] unsignedIntegerValue];
    }
    if (configDictionary[@"ReportCrashesEnabled"] != nil) {
        config.reportCrashesEnabled = [configDictionary[@"ReportCrashesEnabled"] boolValue];
    }
    if (configDictionary[@"TrackLocationEnabled"] != nil) {
        config.trackLocationEnabled = [configDictionary[@"TrackLocationEnabled"] boolValue];
    }
    if (configDictionary[@"LoggingEnabled"] != nil) {
        config.loggingEnabled = [configDictionary[@"LoggingEnabled"] boolValue];
    }
    if (configDictionary[@"HandleFirstActivationAsUpdateEnabled"] != nil) {
        config.handleFirstActivationAsUpdateEnabled = [configDictionary[@"HandleFirstActivationAsUpdateEnabled"] boolValue];
    }
    if (configDictionary[@"PreloadInfo"] != nil) {
        NSDictionary *preloadInfoDictionary = configDictionary[@"PreloadInfo"];
        NSString *trackingID = preloadInfoDictionary[@"TrackingId"];
        YMMYandexMetricaPreloadInfo *preloadInfo =
        [[YMMYandexMetricaPreloadInfo alloc] initWithTrackingIdentifier:trackingID];

        NSDictionary *additionalInfo = preloadInfoDictionary[@"AdditionalInfo"];
        for (NSString *key in additionalInfo) {
            [preloadInfo setAdditionalInfo:additionalInfo[key] forKey:key];
        }

        config.preloadInfo = preloadInfo;
    }

    return config;
}

void ymm_activateWithConfigurationJSON(char *configurationJSON)
{
    if (configurationJSON != NULL) {
        NSString *JSONString = [NSString stringWithUTF8String:configurationJSON];

        NSError *error = nil;
        NSDictionary *configDictionary =
            [NSJSONSerialization JSONObjectWithData:[JSONString dataUsingEncoding:NSUTF8StringEncoding]
                                            options:0
                                              error:&error];

        if (error == nil && [configDictionary isKindOfClass:[NSDictionary class]]) {
            NSString *apiKey = configDictionary[@"ApiKey"];
            if (apiKey.length > 0) {
                YMMYandexMetricaConfiguration *config = ymm_configurationFromDictionary(configDictionary);
                [YMMYandexMetrica activateWithConfiguration:config];
                g_ymm_isAppMetricaActivated = true;
            }
            else {
                NSLog(@"AppMetrica could not be activated with an empty API key.");
            }
        }
    }
}

bool ymm_isAppMetricaActivated()
{
    return g_ymm_isAppMetricaActivated;
}

void ymm_reportEvent(char *message)
{
    if (message != NULL) {
        NSString *messageString = [NSString stringWithUTF8String:message];
        [YMMYandexMetrica reportEvent:messageString onFailure:nil];
    }
}

void ymm_reportEventWithParameters(char *message, char *parameters)
{
    if (message != NULL && parameters != NULL) {
        NSString *messageString = [NSString stringWithUTF8String:message];
        NSString *parametersString = [NSString stringWithUTF8String:parameters];

        NSError *error = nil;
        NSDictionary *parametersDictionary =
            [NSJSONSerialization JSONObjectWithData:[parametersString dataUsingEncoding:NSUTF8StringEncoding]
                                            options:0
                                              error:&error];

        if (error == nil && [parametersDictionary isKindOfClass:[NSDictionary class]]) {
            [YMMYandexMetrica reportEvent:messageString parameters:parametersDictionary onFailure:nil];
        }
    }
}

void ymm_reportError(char *condition, char *stackTrace)
{
    if (condition != NULL && stackTrace != NULL) {
        NSString *conditionString = [NSString stringWithUTF8String:condition];
        NSString *stackTraceString = [NSString stringWithUTF8String:stackTrace];

        NSException *exception = [[NSException alloc] initWithName:kYMMUnityExceptionName
                                                            reason:stackTraceString
                                                          userInfo:nil];
        [YMMYandexMetrica reportError:conditionString exception:exception onFailure:nil];
    }
}

void ymm_setTrackLocationEnabled(bool enabled)
{
    [YMMYandexMetrica setTrackLocationEnabled:(BOOL)enabled];
}

void ymm_setLocation(double latitude, double longitude)
{
    CLLocation *location = [[CLLocation alloc] initWithLatitude:(CLLocationDegrees)latitude
                                                      longitude:(CLLocationDegrees)longitude];
    [YMMYandexMetrica setLocation:location];
}

void ymm_setSessionTimeout(unsigned int sessionTimeoutSeconds)
{
    [YMMYandexMetrica setSessionTimeout:(NSUInteger)sessionTimeoutSeconds];
}

void ymm_setReportCrashesEnabled(bool enabled)
{
    [YMMYandexMetrica setReportCrashesEnabled:(BOOL)enabled];
}

void ymm_setCustomAppVersion(char *appVersion)
{
    if (appVersion != NULL) {
        NSString *appVersionString = [NSString stringWithUTF8String:appVersion];
        [YMMYandexMetrica setCustomAppVersion:appVersionString];
    }
}

void ymm_setLoggingEnabled(bool enabled)
{
    [YMMYandexMetrica setLoggingEnabled:(BOOL)enabled];
}

void ymm_setEnvironmentValue(char *key, char *value)
{
    if (key != NULL && value != NULL) {
        NSString *keyString = [NSString stringWithUTF8String:key];
        NSString *valueString = [NSString stringWithUTF8String:value];

        [YMMYandexMetrica setEnvironmentValue:keyString forKey:valueString];
    }
}

char *ymm_getLibraryVersion()
{
    NSString *version = [[NSString alloc] initWithFormat:@"%d.%d%d", YMM_VERSION_MAJOR, YMM_VERSION_MINOR, YMM_VERSION_PATCH];
    const char *cVersion = [version UTF8String];

    char *res = (char *)malloc(strlen(cVersion) + 1);
    strcpy(res, cVersion);
    return res;
}
