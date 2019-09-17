#!/bin/bash

PROJ_DIR=".."
EXECUTABLE="farmasia-vr.exe"
BUILD_TARGET="Windows64"
BUILD_DIR="Build/${BUILD_TARGET}"
LOG_FILE="build-$(date +%d-%b-%Y).log" # '-' for stdout

# Note that '-nographics' should be removed if GI (Global Illumination) is needed
UNITY_ARGS="-batchmode -nographics -projectPath ${PROJ_DIR} -build${BUILD_TARGET}Player ${BUILD_DIR}/${EXECUTABLE} -quit -logFile ${LOG_FILE}"

fail_exit() {
    echo "BUILD FAILED! See the log file for details: ${LOG_FILE}"
    exit 1
}

load_config() {
    echo "--> Loading configuration file"
    source build.config
    conf_error=0

    if [ -z "${UNITY_PATH}" ]; then
        echo "UNITY_PATH not set in build.config!"
        conf_error=1
    fi

    if [ -z "${UNITY_VERSION}" ]; then
        echo "UNITY_VERSION not set in build.config!"
        conf_error=1
    fi

    [ ${conf_error} -eq 1 ] && exit 1
}

preclean() {
    echo "--> Clearing build directory"
    if [ -d "${PROJ_DIR}/${BUILD_DIR}" ]; then
        rm -r "${PROJ_DIR}/${BUILD_DIR}" || { echo "ERROR: Failed to delete build directory"; exit 1; }
    fi
    mkdir -p "${PROJ_DIR}/${BUILD_DIR}" || { echo "ERROR: Failed to create build directory"; exit 1; }
}

build() {
    echo "--> Creating Unity build"
    "${UNITY_PATH}" ${UNITY_ARGS} || fail_exit
}

# == Program start ==
load_config
preclean
build
echo
echo "Done."