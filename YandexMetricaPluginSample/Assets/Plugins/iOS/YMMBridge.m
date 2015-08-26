#import "YMMBridge.h"
#import "YMMVersion.h"
#import "YMMYandexMetrica.h"

#import <CoreLocation/CoreLocation.h>

static NSString *const kYMMUnityExceptionName = @"UnityException";

void ymm_activateWithAPIKey(char *apiKey)
{
    if (apiKey != NULL) {
        NSString *apiKeyString = [NSString stringWithUTF8String:apiKey];
        [YMMYandexMetrica activateWithApiKey:apiKeyString];
    }
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

void ymm_setEnvironmentValue(char *key, char *value)
{
    if (key != NULL && value != NULL) {
        NSString *keyString = [NSString stringWithUTF8String:key];
        NSString *valueString = [NSString stringWithUTF8String:value];

        [YMMYandexMetrica setEnvironmentValue:keyString forKey:valueString];
    }
}

char* ymm_getLibraryVersion()
{
    NSString * version = [[NSString alloc] initWithFormat:@"%d.%d%d", YMM_VERSION_MAJOR, YMM_VERSION_MINOR, YMM_VERSION_PATCH];
    const char * cVersion = [version UTF8String];
    
    char* res = (char*)malloc(strlen(cVersion) + 1);
    strcpy(res, cVersion);
    return res;
}
