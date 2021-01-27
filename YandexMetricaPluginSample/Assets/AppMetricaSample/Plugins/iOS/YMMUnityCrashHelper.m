NSString *ymm_sample_stringFromCString(char *string)
{
    return string == nil ? nil : [NSString stringWithUTF8String:string];
}

void ymm_sample_crash(char *message)
{
    NSString *messageString = ymm_sample_stringFromCString(message);
    [NSException raise:@"UnityNativeCrash" format:@"%@", messageString];
}

void ymm_sample_crashInOtherLine(char *message)
{
    NSString *messageString = ymm_sample_stringFromCString(message);
    [NSException raise:@"UnityNativeCrash" format:@"%@", messageString];
}

void ymm_sample_otherCrash(char *message)
{
    NSString *messageString = ymm_sample_stringFromCString(message);
    [NSException raise:@"UnityOtherNativeCrash" format:@"%@", messageString];
}
