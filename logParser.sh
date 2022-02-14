#!/bin/bash
# Find "ProjectAuditorCI found X" line and exit with failure code if not X != 0

line="$(grep -E 'ProjectAuditorCI found *' analysis.log)"
issues="$(cut -c 24-24 <<< ${line})"
if [ $issues != "0" ] ; then
  echo "Static analysis found code issues"
  exit 1
fi