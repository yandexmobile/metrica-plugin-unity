#import <AdSupport/ASIdentifierManager.h>
#import <dlfcn.h>

char *ymm_sample_getIdfa()
{
    NSString *idfa = [ASIdentifierManager sharedManager].advertisingIdentifier.UUIDString;
    const char *cIdfa = [idfa UTF8String];

    char *res = (char *)malloc(strlen(cIdfa) + 1);
    strcpy(res, cIdfa);
    return res;
}

bool ymm_sample_isAdvertisingTrackingEnabled()
{
    return [ASIdentifierManager sharedManager].isAdvertisingTrackingEnabled;
}

int ymm_sample_trackingAuthorizationStatus()
{
    if (@available(iOS 14, *)) {
        void *handle = dlopen("AppTrackingTransparency.framework/AppTrackingTransparency", RTLD_LAZY);
        if (handle != NULL) {
            Class gMMSTrackingManager = NSClassFromString(@"ATTrackingManager");
            return (int)[gMMSTrackingManager performSelector:@selector(trackingAuthorizationStatus)];
        }
    }

    return -1;
}
