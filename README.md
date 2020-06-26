# AppMetrica - Unity Plugin

## License
License agreement on use of Yandex AppMetrica SDK is available at [EULA site][LICENSE].

## Documentation
Documentation could be found at [AppMetrica official site][DOCUMENTATION].

## Changelog

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
