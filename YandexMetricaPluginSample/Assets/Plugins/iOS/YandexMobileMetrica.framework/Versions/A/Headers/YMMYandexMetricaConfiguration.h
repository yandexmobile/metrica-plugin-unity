/*
 *  YMMYandexMetricaConfiguration.h
 *
 * This file is a part of the AppMetrica
 *
 * Version for iOS © 2015 YANDEX
 *
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://legal.yandex.com/metrica_termsofuse/
 */

#import <Foundation/Foundation.h>

@class CLLocation;
@class YMMYandexMetricaPreloadInfo;

@interface YMMYandexMetricaConfiguration : NSObject

/** Initialize configuration with specified Application key.
 For invalid Application initialization returns nil in release and raises an exception in debug.

 @param apiKey Application key that is issued during application registration in AppMetrica.
 Application key must be a hexadecimal string in format xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.
 The key can be requested or checked at https://appmetrica.yandex.com
 */
- (instancetype)initWithApiKey:(NSString *)apiKey;

- (instancetype)init __attribute__((unavailable("initWithApiKey: must be used instead.")));

/** Get Application key used to initialize the configuration.
 */
@property (nonatomic, copy, readonly) NSString *apiKey;

/**
 Enable/disable location reporting to AppMetrica.
 If enabled and location set via setLocation: method - that location would be used.
 If enabled and location is not set via setLocation,
 but application has appropriate permission - CLLocationManager would be used to acquire location data.

 Enabled by default.
 */
@property (nonatomic, assign) BOOL trackLocationEnabled;

/** Set/get location to AppMetrica
 To enable AppMetrica to use this location trackLocationEnabled should be 'YES'

 By default is nil
 */
@property (nonatomic, strong) CLLocation *location;

/** Set/get session timeout (in seconds).
 Time limit before the application is considered inactive.
 Minimum accepted value is 10 seconds. All passed values below 10 seconds automatically become 10 seconds.

 By default, the session times out if the application is in background for 10 seconds.
 */
@property (nonatomic, assign) NSUInteger sessionTimeout;

/** Enable/disable tracking app crashes.

 Enabled by default.
 To disable crash tracking, set the parameter value to false.
 */
@property (nonatomic, assign) BOOL reportCrashesEnabled;

/** Set/get the arbitrary application version for AppMetrica to report.

 By default, the application version is set in the app configuration file Info.plist (CFBundleShortVersionString).
 */
@property (nonatomic, copy) NSString *customAppVersion;

/** Enable/disable logging.

 By default logging is disabled.
 */
@property (nonatomic, assign) BOOL loggingEnabled;

/** Set/get preload info, which is used for tracking preload installs.
 Additional info could be https://appmetrica.yandex.com

 By default is nil.
 */
@property (nonatomic, copy) YMMYandexMetricaPreloadInfo *preloadInfo;

@end
