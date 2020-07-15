/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import <YandexMobileMetrica/YandexMobileMetrica.h>
#import <CoreLocation/CoreLocation.h>
#import "YMMBridge.h"

static NSString *const kYMMUnityExceptionName = @"UnityException";

static bool *g_ymm_isAppMetricaActivated = false;

NSString *ymm_stringFromCString(char *string)
{
    return string == nil ? nil : [NSString stringWithUTF8String:string];
}

NSDictionary *ymm_dictionaryFromJSONString(NSString *json, NSError **error)
{
    return json == nil ? nil : [NSJSONSerialization JSONObjectWithData:[json dataUsingEncoding:NSUTF8StringEncoding]
                                                               options:0
                                                                 error:error];
}

BOOL ymm_isDictionaryOrNil(NSDictionary *dictionary)
{
    return dictionary == nil || [dictionary isKindOfClass:[NSDictionary class]];
}

YMMYandexMetricaConfiguration *ymm_configurationFromDictionary(NSDictionary *configDictionary)
{
    if (configDictionary == nil) {
        return nil;
    }
    YMMYandexMetricaConfiguration *config =
    [[YMMYandexMetricaConfiguration alloc] initWithApiKey:configDictionary[@"ApiKey"]];

    if (configDictionary[@"AppVersion"] != nil) {
        config.appVersion = (NSString *)configDictionary[@"AppVersion"];
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
    if (configDictionary[@"CrashReporting"] != nil) {
        config.crashReporting = [configDictionary[@"CrashReporting"] boolValue];
    }
    if (configDictionary[@"LocationTracking"] != nil) {
        config.locationTracking = [configDictionary[@"LocationTracking"] boolValue];
    }
    if (configDictionary[@"Logs"] != nil) {
        config.logs = [configDictionary[@"Logs"] boolValue];
    }
    if (configDictionary[@"HandleFirstActivationAsUpdate"] != nil) {
        config.handleFirstActivationAsUpdate = [configDictionary[@"HandleFirstActivationAsUpdate"] boolValue];
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
    if (configDictionary[@"StatisticsSending"] != nil) {
        config.statisticsSending = [configDictionary[@"StatisticsSending"] boolValue];
    }
    if (configDictionary[@"AppForKids"] != nil) {
        config.appForKids = [configDictionary[@"AppForKids"] boolValue];
    }

    return config;
}

void ymm_activateWithConfigurationJSON(char *configurationJSON)
{
    NSString *configString = ymm_stringFromCString(configurationJSON);

    NSError *error = nil;
    NSDictionary *configDictionary = ymm_dictionaryFromJSONString(configString, &error);

    if (error == nil && ymm_isDictionaryOrNil(configDictionary)) {
        YMMYandexMetricaConfiguration *config = ymm_configurationFromDictionary(configDictionary);
        [YMMYandexMetrica activateWithConfiguration:config];
        g_ymm_isAppMetricaActivated = true;
    }
    else {
        NSLog(@"Invalid configuration json to activate AppMetrica %@", configString);
    }
}

bool ymm_isAppMetricaActivated()
{
    return g_ymm_isAppMetricaActivated;
}

void ymm_reportEvent(char *message)
{
    NSString *messageString = ymm_stringFromCString(message);
    [YMMYandexMetrica reportEvent:messageString onFailure:nil];
}

void ymm_reportEventWithParameters(char *message, char *parameters)
{
    NSString *messageString = ymm_stringFromCString(message);
    NSString *parametersString = ymm_stringFromCString(parameters);

    NSError *error = nil;
    NSDictionary *parametersDictionary = ymm_dictionaryFromJSONString(parametersString, &error);

    if (error == nil && ymm_isDictionaryOrNil(parametersDictionary)) {
        [YMMYandexMetrica reportEvent:messageString parameters:parametersDictionary onFailure:nil];
    }
    else {
        NSLog(@"Invalid parameters json for report event %@", parametersString);
    }
}

void ymm_reportError(char *condition, char *stackTrace)
{
    NSString *conditionString = ymm_stringFromCString(condition);
    NSString *stackTraceString = ymm_stringFromCString(stackTrace);

    NSException *exception = [[NSException alloc] initWithName:kYMMUnityExceptionName
                                                        reason:stackTraceString
                                                      userInfo:nil];
    [YMMYandexMetrica reportError:conditionString exception:exception onFailure:nil];
}

void ymm_setLocationTracking(bool enabled)
{
    [YMMYandexMetrica setLocationTracking:(BOOL)enabled];
}

void ymm_resetLocation()
{
    [YMMYandexMetrica setLocation:nil];
}

void ymm_setLocation(double latitude, double longitude)
{
    CLLocation *location = [[CLLocation alloc] initWithLatitude:(CLLocationDegrees)latitude
                                                      longitude:(CLLocationDegrees)longitude];
    [YMMYandexMetrica setLocation:location];
}

char *ymm_getLibraryVersion()
{
    NSString *version = [[NSString alloc] initWithFormat:@"%d.%d%d", YMM_VERSION_MAJOR, YMM_VERSION_MINOR, YMM_VERSION_PATCH];
    const char *cVersion = [version UTF8String];

    char *res = (char *)malloc(strlen(cVersion) + 1);
    strcpy(res, cVersion);
    return res;
}

void ymm_setUserProfileID(char *userProfileID)
{
    NSString *userProfileIDString = ymm_stringFromCString(userProfileID);
    [YMMYandexMetrica setUserProfileID:userProfileIDString];
}

YMMUserProfileUpdate *ymm_userProfileBirthDateFromDictionary(NSString *methodName, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withAge"]) {
        userProfileUpdate = [[YMMProfileAttribute birthDate] withAge:[values[0] unsignedIntegerValue]];
    }
    else if ([methodName isEqualToString:@"withBirthDate"]) {
        NSDateComponents *date = [[NSDateComponents alloc] init];
        if (values.count >= 1) {
            date.year = [values[0] unsignedIntegerValue];
        }
        if (values.count >= 2) {
            date.month = [values[1] unsignedIntegerValue];
        }
        if (values.count >= 3) {
            date.day = [values[2] unsignedIntegerValue];
        }
        userProfileUpdate = [[YMMProfileAttribute birthDate] withDateComponents:date];
    }
    else if ([methodName isEqualToString:@"withValueReset"]) {
        userProfileUpdate = [[YMMProfileAttribute birthDate] withValueReset];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMGenderType ymm_userProfileGenderTypeFromString(NSString *genderType)
{
    if ([genderType isEqualToString:@"MALE"]) {
        return YMMGenderTypeMale;
    }
    else if ([genderType isEqualToString:@"FEMALE"]) {
        return YMMGenderTypeFemale;
    }
    return YMMGenderTypeOther;
}

YMMUserProfileUpdate *ymm_userProfileGenderFromDictionary(NSString *methodName, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withValue"]) {
        YMMGenderType genderType = ymm_userProfileGenderTypeFromString([values firstObject]);
        userProfileUpdate = [[YMMProfileAttribute gender] withValue:genderType];
    }
    else if ([methodName isEqualToString:@"withValueReset"]) {
        userProfileUpdate = [[YMMProfileAttribute gender] withValueReset];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMUserProfileUpdate *ymm_userProfileNameFromDictionary(NSString *methodName, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withValue"]) {
        userProfileUpdate = [[YMMProfileAttribute name] withValue:[values firstObject]];
    }
    else if ([methodName isEqualToString:@"withValueReset"]) {
        userProfileUpdate = [[YMMProfileAttribute name] withValueReset];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMUserProfileUpdate *ymm_userProfileNotificationsEnabledFromDictionary(NSString *methodName, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withValue"]) {
        userProfileUpdate = [[YMMProfileAttribute notificationsEnabled] withValue:[[values firstObject] boolValue]];
    }
    else if ([methodName isEqualToString:@"withValueReset"]) {
        userProfileUpdate = [[YMMProfileAttribute notificationsEnabled] withValueReset];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMUserProfileUpdate *ymm_userProfileBoolDictionary(NSString *methodName, NSString *key, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withValue"]) {
        userProfileUpdate = [[YMMProfileAttribute customBool:key] withValue:[[values firstObject] boolValue]];
    }
    else if ([methodName isEqualToString:@"withValueIfUndefined"]) {
        userProfileUpdate = [[YMMProfileAttribute customBool:key] withValueIfUndefined:[[values firstObject] boolValue]];
    }
    else if ([methodName isEqualToString:@"withValueReset"]) {
        userProfileUpdate = [[YMMProfileAttribute customBool:key] withValueReset];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMUserProfileUpdate *ymm_userProfileCounterFromDictionary(NSString *methodName, NSString *key, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withDelta"]) {
        userProfileUpdate = [[YMMProfileAttribute customCounter:key] withDelta:[[values firstObject] doubleValue]];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMUserProfileUpdate *ymm_userProfileNumberFromDictionary(NSString *methodName, NSString *key, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withValue"]) {
        userProfileUpdate = [[YMMProfileAttribute customNumber:key] withValue:[[values firstObject] doubleValue]];
    }
    else if ([methodName isEqualToString:@"withValueIfUndefined"]) {
        userProfileUpdate = [[YMMProfileAttribute customNumber:key] withValueIfUndefined:[[values firstObject] doubleValue]];
    }
    else if ([methodName isEqualToString:@"withValueReset"]) {
        userProfileUpdate = [[YMMProfileAttribute customNumber:key] withValueReset];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMUserProfileUpdate *ymm_userProfileStringFromDictionary(NSString *methodName, NSString *key, NSArray *values)
{
    YMMUserProfileUpdate *userProfileUpdate = nil;
    if ([methodName isEqualToString:@"withValue"]) {
        userProfileUpdate = [[YMMProfileAttribute customString:key] withValue:[values firstObject]];
    }
    else if ([methodName isEqualToString:@"withValueIfUndefined"]) {
        userProfileUpdate = [[YMMProfileAttribute customString:key] withValueIfUndefined:[values firstObject]];
    }
    else if ([methodName isEqualToString:@"withValueReset"]) {
        userProfileUpdate = [[YMMProfileAttribute customString:key] withValueReset];
    }
    else {
        NSLog(@"Unknown method %@", methodName);
    }
    return userProfileUpdate;
}

YMMUserProfile *ymm_userProfileFromDictionary(NSDictionary *userProfileDictionary)
{
    if (userProfileDictionary == nil) {
        return nil;
    }
    YMMMutableUserProfile *userProfile = [[YMMMutableUserProfile alloc] init];

    for (int i = 0; i < userProfileDictionary.count; ++i) {
        NSDictionary *userProfileUpdateDictionary = userProfileDictionary[[@(i) stringValue]];
        NSString *attributeName = userProfileUpdateDictionary[@"AttributeName"];
        NSString *methodName = userProfileUpdateDictionary[@"MethodName"];
        NSString *key = userProfileUpdateDictionary[@"Key"];
        NSArray *values = [userProfileUpdateDictionary mutableArrayValueForKey:@"Values"];
        if ([attributeName isEqualToString:@"birthDate"]) {
            [userProfile apply:ymm_userProfileBirthDateFromDictionary(methodName, values)];
        }
        else if ([attributeName isEqualToString:@"gender"]) {
            [userProfile apply:ymm_userProfileGenderFromDictionary(methodName, values)];
        }
        else if ([attributeName isEqualToString:@"name"]) {
            [userProfile apply:ymm_userProfileNameFromDictionary(methodName, values)];
        }
        else if ([attributeName isEqualToString:@"notificationsEnabled"]) {
            [userProfile apply:ymm_userProfileNotificationsEnabledFromDictionary(methodName, values)];
        }
        else if ([attributeName isEqualToString:@"customBoolean"]) {
            [userProfile apply:ymm_userProfileBoolDictionary(methodName, key, values)];
        }
        else if ([attributeName isEqualToString:@"customCounter"]) {
            [userProfile apply:ymm_userProfileCounterFromDictionary(methodName, key, values)];
        }
        else if ([attributeName isEqualToString:@"customNumber"]) {
            [userProfile apply:ymm_userProfileNumberFromDictionary(methodName, key, values)];
        }
        else if ([attributeName isEqualToString:@"customString"]) {
            [userProfile apply:ymm_userProfileStringFromDictionary(methodName, key, values)];
        }
        else {
            NSLog(@"Unknown attribute %@", attributeName);
        }
    }
    return userProfile;
}

void ymm_reportUsertProfileJSON(char *userProfileJSON)
{
    NSString *userProfileString = ymm_stringFromCString(userProfileJSON);

    NSError *error = nil;
    NSDictionary *userProfileDictionary = ymm_dictionaryFromJSONString(userProfileString, &error);

    if (error == nil && ymm_isDictionaryOrNil(userProfileDictionary)) {
        YMMUserProfile *userProfile = ymm_userProfileFromDictionary(userProfileDictionary);
        [YMMYandexMetrica reportUserProfile:userProfile onFailure:nil];
    }
    else {
        NSLog(@"Invalid userProfile json %@", userProfileString);
    }
}

YMMRevenueInfo *ymm_revenueFromDictionary(NSDictionary *revenueDictionary)
{
    if (revenueDictionary == nil) {
        return nil;
    }
    NSString *currency = revenueDictionary[@"Currency"];
    YMMMutableRevenueInfo *revenue = nil;
    if (revenueDictionary[@"PriceDecimal"] != nil) {
        NSString *priceDecimalString = revenueDictionary[@"PriceDecimal"];
        NSDictionary *locale = [NSDictionary dictionaryWithObject:@"." forKey:NSLocaleDecimalSeparator];
        NSDecimalNumber *priceDecimal = [NSDecimalNumber decimalNumberWithString:priceDecimalString locale:locale];
        revenue = [[YMMMutableRevenueInfo alloc] initWithPriceDecimal:priceDecimal currency:currency];
    }
    else {
        double price = [revenueDictionary[@"Price"] doubleValue];
        revenue = [[YMMMutableRevenueInfo alloc] initWithPrice:price currency:currency];
    }

    if (revenueDictionary[@"Quantity"] != nil) {
        [revenue setQuantity:[revenueDictionary[@"Quantity"] unsignedIntegerValue]];
    }
    [revenue setProductID:revenueDictionary[@"ProductID"]];
    [revenue setPayload:ymm_dictionaryFromJSONString(revenueDictionary[@"Payload"], nil)];

    if (revenueDictionary[@"Receipt"] != nil) {
        NSDictionary *receiptDictionary = revenueDictionary[@"Receipt"];
        [revenue setReceiptData:[[NSData alloc] initWithBase64EncodedString:receiptDictionary[@"Data"] options:0]];
        [revenue setTransactionID:receiptDictionary[@"TransactionID"]];
    }
    return revenue;
}

void ymm_reportRevenueJSON(char *revenueJSON)
{
    NSString *revenueString = ymm_stringFromCString(revenueJSON);

    NSError *error = nil;
    NSDictionary *revenueDictionary = ymm_dictionaryFromJSONString(revenueString, &error);

    if (error == nil && ymm_isDictionaryOrNil(revenueDictionary)) {
        YMMRevenueInfo *revenue = ymm_revenueFromDictionary(revenueDictionary);
        [YMMYandexMetrica reportRevenue:revenue onFailure:nil];
    }
    else {
        NSLog(@"Invalid revenue json %@", revenueString);
    }
}

void ymm_setStatisticsSending(bool enabled)
{
    [YMMYandexMetrica setStatisticsSending:(BOOL)enabled];
}

void ymm_sendEventsBuffer()
{
    [YMMYandexMetrica sendEventsBuffer];
}

char *ymm_stringFromRequestDeviceIDError(NSError *error)
{
    if (error == nil) {
        return nil;
    }
    if ([error.domain isEqualToString:NSURLErrorDomain]) {
        return "NETWORK";
    }
    return "UNKNOWN";
}

void ymm_requestAppMetricaDeviceID(YMMRequestDeviceIDCallbackDelegate callbackDelegate, YMMAction actionPtr)
{
    [YMMYandexMetrica requestAppMetricaDeviceIDWithCompletionQueue:nil completionBlock:^(NSString * _Nullable appMetricaDeviceID, NSError * _Nullable error) {
        if (error != nil) {
            NSLog(@"Error request AppMetrica DeviceID: %@", error.description);
        }
        if (callbackDelegate != nil) {
            callbackDelegate(actionPtr, [appMetricaDeviceID UTF8String], ymm_stringFromRequestDeviceIDError(error));
        }
    }];
}

void ymm_reportReferralUrl(char *referralUrl)
{
    NSString *referralUrlString = ymm_stringFromCString(referralUrl);
    [YMMYandexMetrica reportReferralUrl:[NSURL URLWithString:referralUrlString]];
}

void ymm_reportAppOpen(char *deeplink)
{
    NSString *deeplinkString = ymm_stringFromCString(deeplink);
    [YMMYandexMetrica handleOpenURL:[NSURL URLWithString:deeplinkString]];
}
