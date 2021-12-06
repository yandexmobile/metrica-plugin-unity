/*
 * Version for iOS
 * © 2012–2020 YANDEX
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

NS_ASSUME_NONNULL_BEGIN

/** The default implementation of the `YMMErrorRepresentable` protocol.
 */
@interface YMMError : NSObject <YMMErrorRepresentable>

/** Creates the error instance with its ID.
 
 @note For more information, see `YMMErrorRepresentable`.
 
 @param identifier Unique error identifier
 @return The `YMMError` instance
 */
+ (instancetype)errorWithIdentifier:(NSString *)identifier;

/** Creates the error instance with its ID and other properties.
 
 @note For more information on parameters, see `YMMErrorRepresentable`.
 
 @param identifier Unique error identifier
 @param message Arbitrary description of the error
 @param parameters Addittional error parameters
 @return The `YMMError` instance
 */
+ (instancetype)errorWithIdentifier:(NSString *)identifier
                            message:(nullable NSString *)message
                         parameters:(nullable NSDictionary<NSString *, id> *)parameters;

/** Creates the error instance with its ID and other properties.

 @note For more information on parameters, see `YMMErrorRepresentable`.

 @param identifier Unique error identifier
 @param message Arbitrary description of the error
 @param parameters Addittional error parameters
 @param backtrace Custom error backtrace
 @param underlyingError Underlying error instance that conforms to the `YMMErrorRepresentable` protocol
 @return The `YMMError` instance
 */
+ (instancetype)errorWithIdentifier:(NSString *)identifier
                            message:(nullable NSString *)message
                         parameters:(nullable NSDictionary<NSString *, id> *)parameters
                          backtrace:(nullable NSArray<NSNumber *> *)backtrace
                    underlyingError:(nullable id<YMMErrorRepresentable>)underlyingError;

- (instancetype)init NS_UNAVAILABLE;
+ (instancetype)new NS_UNAVAILABLE;

@end

NS_ASSUME_NONNULL_END
