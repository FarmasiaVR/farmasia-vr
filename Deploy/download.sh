#!/bin/bash

OUTPUT_DIR="Farmasia"

check_prerequisites() {
    echo "--> Checking prerequisites"
    fail=0
    which unzip 2>/dev/null || { echo "ERROR: unzip is not installed!"; fail=1; }
    which gdrive 2>/dev/null || { echo "ERROR: gdrive is not installed!"; fail=1; }
    [ ${fail} -eq 1 ] && exit 1
}

download_zip() {
    echo "--> Downloading zip from Google Drive"
    download_id=$(gdrive list | awk 'NR==2 {print $1; exit}')
    download_name=$(gdrive list | awk 'NR==2 {print $2; exit}')
    ZIP_FILE="${download_name}"

    if [ -z "${download_id}" ]; then
        echo "ERROR: File not found"
        exit 1;
    fi

    echo "  --> Create temporary directory"
    TMP_DIR=$(mktemp -d)
    [ -d "${TMP_DIR}" ] || { echo "ERROR: Failed to create temporary directory!"; exit 1; }

    echo "  --> Downloading '${download_name}'"
    gdrive download --path "${TMP_DIR}" "${download_id}"
}

unzip_zip() {
    echo "--> Unzipping zip file to output directory"
    echo "  --> Ensure output directory exists"
    mkdir -p "${OUTPUT_DIR}"

    echo "  --> Unzipping zip file"
    unzip -u -o "${TMP_DIR}/${ZIP_FILE}" -d "${OUTPUT_DIR}" >/dev/null 2>&1

    echo "  --> Deleting temporary directory"
    rm -r "${TMP_DIR}"
}

check_prerequisites
download_zip
unzip_zip
echo
echo "Done."
