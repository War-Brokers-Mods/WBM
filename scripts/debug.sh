#!/bin/bash

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

cd "$SCRIPT_DIR" || {
	echo "Can not locate script directory"
	exit
}
cd ..
source ./scripts/config.sh

dotnet build
rm "$WB_PLUGINS_DIR/WBM/WBM.dll"
mkdir -p "$WB_PLUGINS_DIR/WBM"
cp ./WBM/bin/Debug/net48/WBM.dll "$WB_PLUGINS_DIR/WBM/WBM.dll"
cp -R ./assets "$WB_PLUGINS_DIR/WBM"
