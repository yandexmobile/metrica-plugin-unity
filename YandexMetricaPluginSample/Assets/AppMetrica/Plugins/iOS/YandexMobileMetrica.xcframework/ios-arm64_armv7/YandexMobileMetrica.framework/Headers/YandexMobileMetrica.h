/*
 * Version for iOS
 * © 2012–2019 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if __has_include("YMMYandexMetrica.h")
    #import "YMMYandexMetrica.h"
    #import "YMMVersion.h"
    #import "YMMYandexMetricaConfiguration.h"
    #import "YMMReporterConfiguration.h"
    #import "YMMYandexMetricaReporting.h"
    #import "YMMYandexMetricaPreloadInfo.h"
    #import "YMMProfileAttribute.h"
    #import "YMMUserProfile.h"
    #import "YMMRevenueInfo.h"
    #import "YMMError.h"
    #import "YMMErrorRepresentable.h"
    #import "YMMECommerce.h"
#else
    #import <YandexMobileMetrica/YMMYandexMetrica.h>
    #import <YandexMobileMetrica/YMMVersion.h>
    #import <YandexMobileMetrica/YMMYandexMetricaConfiguration.h>
    #import <YandexMobileMetrica/YMMReporterConfiguration.h>
    #import <YandexMobileMetrica/YMMYandexMetricaReporting.h>
    #import <YandexMobileMetrica/YMMYandexMetricaPreloadInfo.h>
    #import <YandexMobileMetrica/YMMProfileAttribute.h>
    #import <YandexMobileMetrica/YMMUserProfile.h>
    #import <YandexMobileMetrica/YMMRevenueInfo.h>
    #import <YandexMobileMetrica/YMMError.h>
    #import <YandexMobileMetrica/YMMErrorRepresentable.h>
    #import <YandexMobileMetrica/YMMECommerce.h>
#endif
