#!/bin/bash

ZIP_FILE="farmasia-vr.zip"
PROJ_DIR=".."
BUILD_DIR="${PROJ_DIR}/Build/Windows64"
OUTPUT_DIR="${PROJ_DIR}/Build"

check_prerequisites() {
    echo "--> Checking prerequisites"
    fail=0
    which zip 2>/dev/null || { echo "ERROR: zip is not installed!"; fail=1; }
    which gdrive 2>/dev/null || { echo "ERROR: gdrive is not installed!"; fail=1; }
    [ ${fail} -eq 1 ] && exit 1
}

create_zip() {
    echo "--> Zipping build directory"
    zip -r "${OUTPUT_DIR}/${ZIP_FILE}" "${BUILD_DIR}" >/dev/null 2>&1
    [ $? -eq 0 ] || { echo "FAIL"; exit 1; }
}

upload_zip() {
    echo "--> Uploading zip to Google Drive"
    prev_upload_id=$(gdrive list | awk 'NR==2 {print $1; exit}')

    # Upload new file if prev_upload_id is empty
    # Else, update existing file
    if [ -z "${prev_upload_id}" ]; then
        gdrive upload "${OUTPUT_DIR}/${ZIP_FILE}"
    else
        gdrive update "${prev_upload_id}" "${OUTPUT_DIR}/${ZIP_FILE}"
    fi
}

check_prerequisites
create_zip
upload_zip
echo
echo "Done."