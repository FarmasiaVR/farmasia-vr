#!/bin/bash

ZIP_FILE="farmasia-vr.zip"
BUILD_DIR="Build/Windows64"
OUTPUT_DIR="Build"

echo ":: Zipping build directory"
zip -r "${OUTPUT_DIR}/${ZIP_FILE}" "${BUILD_DIR}" >/dev/null 2>&1

echo "The zip can be found at $(readlink -f ${OUTPUT_DIR}/${ZIP_FILE})"
echo
echo "Done."