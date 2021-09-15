#!/bin/bash

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

cd "$SCRIPT_DIR" || {
	echo "Can not locate script directory"
	exit
}
cd ..
source ./scripts/config.sh

dotnet build --configuration Release
[ -e ./dist ] && rm -rf ./dist
mkdir ./dist
cp ./WBM/bin/Release/net48/WBM.dll ./dist/WBM.dll
cp ./WBM/bin/Release/net48/websocket-sharp.dll ./dist/websocket-sharp.dll
cp ./WBM/dll/ConfigurationManager.dll ./dist/ConfigurationManager.dll
cp -R ./assets ./dist/assets

# create new zip file
[ -e ./WBM.zip ] && rm ./WBM.zip
cd ./dist
zip -r WBM.zip ./*
mv ./WBM.zip ..
cd ..
rm -rf ./dist
