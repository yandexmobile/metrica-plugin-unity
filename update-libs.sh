#!/bin/bash

set -e

NATIVE_SDK_DIR='YandexMetricaPluginSample/Assets/AppMetrica/Plugins'

###############
### Android ###
###############
updateAndroid() {
	echo "Update AppMetrca SDK Android"

	local libsDir="$NATIVE_SDK_DIR/Android"
	local repoUrl='https://oss.sonatype.org/content/groups/public/com/yandex/android/mobmetricalib'
	local oldVersion=$(ls "$libsDir" | grep mobmetricalib | sed -n "1s/mobmetricalib-\(.*\).aar/\1/p")
	local newVersion=$(curl $repoUrl/maven-metadata.xml 2>/dev/null | grep latest | sed -n 's/ *<latest>\(.*\)<.latest>/\1/p')

	if [[ "$oldVersion" == "$newVersion" ]]; then
	    echo "AppMetrica is up to date (v$newVersion)"
	    return 0
	fi

	local oldFileName=mobmetricalib-${oldVersion}.aar
	local newFileName=mobmetricalib-${newVersion}.aar

	echo "Downloading AppMetrica v$newVersion"
	local downloadUrl=$repoUrl/$newVersion/mobmetricalib-${newVersion}.aar
	echo "URL: $downloadUrl"
	wget -O "$libsDir/$newFileName" -- "$downloadUrl" #2>/dev/null

	echo "Removing old AppMetrica: $oldFileName"
	rm "$libsDir/$oldFileName"

	echo "Updating meta file"
	mv "$libsDir/${oldFileName}.meta" "$libsDir/${newFileName}.meta"
}

###########
### iOS ###
###########
updateIos() {
	echo "Update AppMetrica SDK iOS"

	pod repo update

	local libsDir="$NATIVE_SDK_DIR/iOS"
	local metricaSpec=$(pod spec cat YandexMobileMetrica)
	local tmpDir=update_tmp

	echo "Creating temp directory '$tmpDir'"
	rm -rf "$tmpDir" && mkdir "$tmpDir"
	cd "$tmpDir"

	echo "Downloading YandexMobileMetrica framework"
	local sdkZipUrl=$(echo "$metricaSpec" | python -c 'import json,sys;print(json.load(sys.stdin)["source"]["http"])')
	echo "URL: $sdkZipUrl"
	wget -q -O framework.zip -- "$sdkZipUrl"
	unzip -q framework.zip

	echo "Removing old framework"
	rm -rd ../$libsDir/*.framework*

	echo "Copying framework"
	cp -R static/* ../$libsDir

	echo "Resolving symlinks"
	for frameworkDir in ../$libsDir/*.framework; do
		local frameworkName=$(echo $frameworkDir | sed -n "s/.*iOS\/\(.*\).framework/\1/p")
	    rm -rd "$frameworkDir/Headers"
	    cp -R "$frameworkDir/Versions/A/Headers" "$frameworkDir/Headers"
	    rm "$frameworkDir/$frameworkName"
	    cp "$frameworkDir/Versions/A/$frameworkName" "$frameworkDir/$frameworkName"
	    chmod +x "$frameworkDir/$frameworkName"
	    rm -rd "$frameworkDir/Versions"
	done

	cd ..

	echo "Removing temp directory '$tmpDir'"
	rm -rf "$tmpDir"
}

############
### Main ###
############
updateAndroid
updateIos
echo "Open a project in Unity to create .meta files"
