/*
 * Version for iOS
 * © 2012–2017 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

#if __has_include("YMMYandexMetrica.h")
    #import "YMMYandexMetrica.h"
    #import "YMMVersion.h"
    #import "YMMYandexMetricaConfiguration.h"
    #import "YMMYandexMetricaReporting.h"
    #import "YMMYandexMetricaPreloadInfo.h"
#else
    #import <YandexMobileMetrica/YMMYandexMetrica.h>
    #import <YandexMobileMetrica/YMMVersion.h>
    #import <YandexMobileMetrica/YMMYandexMetricaConfiguration.h>
    #import <YandexMobileMetrica/YMMYandexMetricaReporting.h>
    #import <YandexMobileMetrica/YMMYandexMetricaPreloadInfo.h>
#endif
