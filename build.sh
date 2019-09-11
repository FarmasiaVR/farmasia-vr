#!/bin/bash

EXECUTABLE="farmasia-vr.exe"
BUILD_TARGET="Windows64"
BUILD_DIR="Build/${BUILD_TARGET}/"
LOG_FILE="${BUILD_DIR}/build-$(date +%d-%b-%Y).log" # '-' for stdout

UNITY_VERSION="2019.2.3f1"
UNITY_PATH="/c/Program Files/Unity/Hub/Editor/${UNITY_VERSION}/Editor/Unity.exe"

# Note that '-nographics' should be removed if GI (Global Illumination) is needed
UNITY_ARGS="-batchmode -nographics -projectPath . -build${BUILD_TARGET}Player ${BUILD_DIR}/${EXECUTABLE} -quit -logFile ${LOG_FILE}"

echo "--> Clearing build directory"
mkdir -p ${BUILD_DIR}
rm -r ${BUILD_DIR}/*

# Temp/ is left if the previous build failed for some reason
echo "--> Deleting temp directory"
[ -d Temp ] && rm -r Temp

echo "--> Creating Unity build"
"${UNITY_PATH}" ${UNITY_ARGS} && echo "Done." && exit

echo "BUILD FAILED! See the log file for details: ${LOG_FILE}"
