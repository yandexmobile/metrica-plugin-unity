/*
 * Version for iOS
 * © 2012–2019 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#import <Foundation/Foundation.h>

#if __has_include("YMMErrorRepresentable.h")
    #import "YMMErrorRepresentable.h"
#else
    #import <YandexMobileMetrica/YMMErrorRepresentable.h>
#endif

@class YMMUserProfile;
@class YMMRevenueInfo;

NS_ASSUME_NONNULL_BEGIN

/** YMMYandexMetricaReporting protocol groups methods that are used by custom reporting objects.
 */
@protocol YMMYandexMetricaReporting <NSObject>

/** Reports a custom event.

 @param name Short name or description of the event.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportEvent:(NSString *)name
          onFailure:(nullable void (^)(NSError *error))onFailure;

/** Reports a custom event with additional parameters.

 @param name Short name or description of the event.
 @param params Dictionary of name/value pairs that must be sent to the server.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportEvent:(NSString *)name
         parameters:(nullable NSDictionary *)params
          onFailure:(nullable void (^)(NSError *error))onFailure;

/** Reports custom error messages.

 @param name Short name or description of the error.
 @param exception NSException object that must be sent to the server.
 @param onFailure Block to be executed if an error occurs while reporting, the error is passed as block argument.
 */
- (void)reportError:(NSString *)name
          exception:(nullable NSException *)exception
          onFailure:(nullable void (^)(NSError *error))onFailure
DEPRECATED_MSG_ATTRIBUTE("Use reportError:options:onFailure: or reportNSError:options:onFailure:");

/** Reports an error of the `NSError` type.
 AppMetrica uses domain and code for grouping errors.
 
 Limits for `NSError`:
 - 200 characters for `domain`;
 - 50 key-value pairs for `userInfo`. 100 characters for a key length, 2000 for a value length;
 - 10 underlying errors using `NSUnderlyingErrorKey` as a key in `userInfo`;
 - 200 stack frames in a backtrace using `YMMBacktraceErrorKey` as a key in `userInfo`.
 If the value exceeds the limit, AppMetrica truncates it.
 
 @note You can also report custom backtrace in `NSError`, see the `YMMBacktraceErrorKey` constant.

 @param error The error to report.
 @param onFailure Block to be executed if an error occurres while reporting, the error is passed as block argument.
 */
- (void)reportNSError:(NSError *)error
            onFailure:(nullable void (^)(NSError *error))onFailure NS_SWIFT_NAME(report(nserror:onFailure:));

/** Reports an error of the `NSError` type.
 AppMetrica uses domain and code for grouping errors.
 Use this method to set the reporting options.
 
 Limits for `NSError`:
 - 200 characters for `domain`;
 - 50 key-value pairs for `userInfo`. 100 characters for a key length, 2000 for a value length;
 - 10 underlying errors using `NSUnderlyingErrorKey` as a key in `userInfo`;
 - 200 stack frames in a backtrace using `YMMBacktraceErrorKey` as a key in `userInfo`.
 If the value exceeds the limit, AppMetrica truncates it.
 
 @note You can also report custom backtrace in `NSError`, see the `YMMBacktraceErrorKey` constant.

 @param error The error to report.
 @param options The options of error reporting.
 @param onFailure Block to be executed if an error occurres while reporting, the error is passed as block argument.
 */
- (void)reportNSError:(NSError *)error
              options:(YMMErrorReportingOptions)options
            onFailure:(nullable void (^)(NSError *error))onFailure NS_SWIFT_NAME(report(nserror:options:onFailure:));

/** Reports a custom error.
 @note See `YMMErrorRepresentable` for more information.

 @param error The error to report.
 @param onFailure Block to be executed if an error occurres while reporting, the error is passed as block argument.
 */
- (void)reportError:(id<YMMErrorRepresentable>)error
          onFailure:(nullable void (^)(NSError *error))onFailure NS_SWIFT_NAME(report(error:onFailure:));

/** Reports a custom error.
 Use this method to set the reporting options.
 @note See `YMMErrorRepresentable` for more information.

 @param error The error to report.
 @param options The options of error reporting.
 @param onFailure Block to be executed if an error occurres while reporting, the error is passed as block argument.
 */
- (void)reportError:(id<YMMErrorRepresentable>)error
            options:(YMMErrorReportingOptions)options
          onFailure:(nullable void (^)(NSError *error))onFailure NS_SWIFT_NAME(report(error:options:onFailure:));

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

/** Pauses current session.
 All events reported after calling this method and till the session timeout will still join this session.
 Should be used when auto tracking of application state is unavailable or is different.
 */
- (void)pauseSession;

/** Enables/disables statistics sending to the AppMetrica server.

 @note Disabling this option doesn't affect data sending from the main apiKey.

 @param enabled Flag indicating whether the statistics sending is enabled. By default, the sending is enabled.
 */
- (void)setStatisticsSending:(BOOL)enabled;

/** Sends all stored events from the buffer.
 
 AppMetrica SDK doesn't send events immediately after they occurred. It stores events data in the buffer.
 This method sends all the data from the buffer and flushes it.
 Use the method to force stored events sending after important checkpoints of user scenarios.

 @warning Frequent use of the method can lead to increasing outgoing internet traffic and energy consumption.
 */
- (void)sendEventsBuffer;

@end

NS_ASSUME_NONNULL_END
