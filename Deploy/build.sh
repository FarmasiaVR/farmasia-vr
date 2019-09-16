#!/bin/bash

PROJ_DIR=".."
EXECUTABLE="farmasia-vr.exe"
BUILD_TARGET="Windows64"
BUILD_DIR="${PROJ_DIR}/Build/${BUILD_TARGET}"
LOG_FILE="${BUILD_DIR}/build-$(date +%d-%b-%Y).log" # '-' for stdout

UNITY_VERSION="2019.2.3f1"
UNITY_PATH="/c/Program Files/Unity/Hub/Editor/${UNITY_VERSION}/Editor/Unity.exe"

# Note that '-nographics' should be removed if GI (Global Illumination) is needed
UNITY_ARGS="-batchmode -nographics -projectPath ${PROJ_DIR} -build${BUILD_TARGET}Player ${BUILD_DIR}/${EXECUTABLE} -quit -logFile ${LOG_FILE}"

fail_exit() {
    echo "BUILD FAILED! See the log file for details: ${LOG_FILE}"
    exit 1
}

# == Program start ==
echo "--> Clearing build directory"
mkdir -p ${BUILD_DIR} || fail_exit
rm -r ${BUILD_DIR}/* || fail_exit

echo "--> Creating Unity build"
"${UNITY_PATH}" ${UNITY_ARGS} || fail_exit
echo "Done."