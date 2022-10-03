# AppMetrica - Unity Plugin

## License
License agreement on use of Yandex AppMetrica SDK is available at [EULA site][LICENSE].

## Documentation
Documentation could be found at [AppMetrica official site][DOCUMENTATION].

## Changelog

### Version 5.2.0

* Updated native SDKs *(iOS 4.4.0, Android 5.2.0)*
* Supported AdRevenue
* Added class YandexAppMetricaAdRevenue
* Added a method to report ad revenue `ReportAdRevenue(YandexAppMetricaAdRevenue adRevenue)`

### Version 5.1.0

* Updated native SDKs *(Android 5.0.1)*
* Exceptions from Application.logMessageReceived are sent as errors

### Version 5.0.0

* Updated native SDKs *(Android 5.0.0)*
* Removed LocationService.Start call
* Removed use of APP_METRICA_TRACK_LOCATION_DISABLED define

### Version 4.3.0

* Updated native SDKs *(iOS 4.2.0, Android 4.2.0)*
* Supported [EDM4U](https://github.com/googlesamples/unity-jar-resolver) for dependency resolution
* Crash handling improved
* Added methods for error reporting `ReportError(Exception exception, string condition)`, `ReportErrorFromLogCallback(string condition, string stackTrace)`
* Added method for crash reporting `ReportUnhandledException(Exception exception)`
* Deprecared methods `ReportError(string condition, string stackTrace)`. Use the `ReportError(Exception exception, string condition)` instead
* `ReportError(string groupIdentifier, string condition, string stackTrace)`. Use the `ReportError(string groupIdentifier, string condition, Exception exception)` instead

### Version 4.2.0

* Fixed a problem with uploading app in AppStore (`YandexMobileMetrica.framework/YandexMobileMetrica' is not permitted. Your app can’t contain standalone executables or libraries, other than a valid CFBundleExecutable of supported bundles.`)

### Version 4.1.0

* Updated native SDKs *(Android 4.1.1)*

### Version 4.0.0

* Updated native SDKs *(iOS 4.0.0, Android 4.0.0)*
* Remove InstalledAppCollecting from YandexAppMetricaConfig
* Added property RevenueAutoTrackingEnabled to YandexAppMetricaConfig for disable auto tracking revenue
* Added dependency from [Install Referrer Library](https://developer.android.com/google/play/installreferrer/library) v2.2

### Version 3.8.0

* Updated native SDKs *(iOS 3.16.0, Android 3.21.1)*
* Added a method for requesting access to IDFA

### Version 3.7.0

* Updated native SDKs *(iOS 3.14.0, Android 3.18.0)*
* Added a method to report event with json string value
* Added a method to set error environment
* Added a method to report error with identifier

### Version 3.6.0

* Updated native SDKs *(iOS 3.11.1, Android 3.14.3)*
* Added the configuration property AppForKids for applications from App Store Kids' Category.

### Version 3.5.1

* Added a method to report referral url
* Added a method to report open url
* Added property PriceDecimal to YandexAppMetricaRevenue. Use it instead of deprecated Price
* Simplified connection of the iAd framework for iOS

### Version 3.5.0

* Updated native SDKs *(iOS 3.9.4, Android 3.13.1)*

### Version 3.4.0

* Updated native SDKs *(iOS 3.7.1, Android 3.6.4)*

### Version 3.3.0

* Updated native SDKs *(iOS 3.6.0, Android 3.6.0)*

### Version 3.2.0

* Updated native SDKs *(iOS 3.4.0, Android 3.4.0)*
* Fixed AppMetrica dependency integration

### Version 3.1.0

* Updated native SDKs *(iOS 3.2.0, Android 3.2.2)*
* Added a method to disable statistics sending.
* Added a method to retrieve the AppMetrica device ID.
* Added a method to force stored events sending.

### Version 3.0.1

* Updated native *iOS SDK 3.1.2*

### Version 3.0.0

* Updated native SDKs *(iOS 3.1.1, Android 3.1.0)*
* Added user profiles
* Added revenue tracking
* Unified and revised the API

### Version 2.1

* Updated native SDKs *(iOS 2.9.8)*

### Version 2.0.0

* Updated native SDKs *(iOS 2.7.0, Android 2.62)*
* Fixed plugin files structure: everything moved into `Assets/AppMetrica`
* Added method for activation configuration updates handling *(for AppMetrica Push SDK Unity Plugin)*

### Version 1.20

* Updated native SDKs *(iOS 2.5.1, Android 2.42)*

### Version 1.10

* Updated native SDKs *(iOS 2.4.0, Android 2.40)*
* Fixed critical iOS problems *(bitcode support, protobuf lib related crash)*
* Added availability to disable location gathering with compiler directive **APP_METRICA_TRACK_LOCATION_DISABLED**

### Version 1.0

* Initial release with iOS and Android native SDK *(iOS 2.0, Android 2.0)*

## Notice

The directory **MetricaPluginSample** includes sample with plugin for Unity 5+. You should copy file  **Other/Unity.iOS.Extensions.Xcode.dll** into **MetricaPluginSample/Assets/YandexMobileMetrica/Editor** to launch sample for Unity 4.

[LICENSE]: https://yandex.com/legal/appmetrica_sdk_agreement/ "Yandex AppMetrica agreement"
[DOCUMENTATION]: https://appmetrica.yandex.com/docs/mobile-sdk-dg/concepts/unity-plugin.html "Yandex AppMetrica Unity Plugin documentation"
