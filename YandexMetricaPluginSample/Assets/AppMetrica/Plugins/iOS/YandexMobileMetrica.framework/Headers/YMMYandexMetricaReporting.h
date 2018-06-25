/*
 * Version for iOS
 * © 2012–2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import <Foundation/Foundation.h>

@class YMMUserProfile;
@class YMMRevenueInfo;

NS_ASSUME_NONNULL_BEGIN

/** YMMYandexMetricaReporting protocol groups methods that are used by custom reporting objects.
 */
@protocol YMMYandexMetricaReporting <NSObject>

/** Reporting custom event.

 @param name Short name or description of the event.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportEvent:(NSString *)name
          onFailure:(nullable void (^)(NSError *error))onFailure;

/** Reporting custom event with additional parameters.

 @param name Short name or description of the event.
 @param params Dictionary of name/value pairs that must be sent to the server.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportEvent:(NSString *)name
         parameters:(nullable NSDictionary *)params
          onFailure:(nullable void (^)(NSError *error))onFailure;

/** Reporting custom error messages.

 @param name Short name or description of the error.
 @param exception NSException object that must be sent to the server.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportError:(NSString *)name
          exception:(nullable NSException *)exception
          onFailure:(nullable void (^)(NSError *error))onFailure;

/** Sends information about the user profile.

 @param userProfile The `YMMUserProfile` object. Contains user profile information.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportUserProfile:(YMMUserProfile *)userProfile
                onFailure:(nullable void (^)(NSError *error))onFailure;

/** Sends information about the purchase.

 @param revenueInfo The `YMMRevenueInfo` object. Contains purchase information.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportRevenue:(YMMRevenueInfo *)revenueInfo
            onFailure:(nullable void (^)(NSError *error))onFailure;

/** Sets the ID of the user profile.

 @warning The value can contain up to 200 characters.
 @param userProfileID The custom user profile ID.
 */
- (void)setUserProfileID:(nullable NSString *)userProfileID;

/** Resumes last session or creates a new one if it has been expired.
 Should be used when auto tracking of application state is unavailable or is different.
 */
- (void)resumeSession;

/** Pause current session.
 All events reported after calling this method and till the session timeout will still join this session.
 Should be used when auto tracking of application state is unavailable or is different.
 */
- (void)pauseSession;

@end

NS_ASSUME_NONNULL_END
