#AppMetrica - Unity Plugin

## License
License agreement on use of Yandex AppMetrica for Apps SDK is available at [EULA site] [LICENSE].

## Documentation
Documentation could be found at [metrica official site] [DOCUMENTATION].

## Changelog

### Version 1.10

* Updated native SDKs *(iOS 2.4.0, Android 2.40)*
* Fixed critical iOS problems *(bitcode support, protobuf lib related crash)*
* Added availability to disable location gathering with compiler directive **APP_METRICA_TRACK_LOCATION_DISABLED**

### Version 1.0

* Initial release with iOS and Android native SDK *(iOS 2.0, Android 2.0)*

## Notice

The directory **MetricaPluginSample** includes sample with plugin for Unity 5+. You should copy file  **Other/Unity.iOS.Extensions.Xcode.dll** into **MetricaPluginSample/Assets/YandexMobileMetrica/Editor** to launch sample for Unity 4.

[LICENSE]: http://legal.yandex.ru/metrica_termsofuse/ "Yandex AppMetrica agreement"
[DOCUMENTATION]: https://tech.yandex.com/metrica-mobile-sdk/doc/mobile-sdk-dg/concepts/unity-plugin-docpage/ "Yandex AppMetrica Unity Plugin documentation"
