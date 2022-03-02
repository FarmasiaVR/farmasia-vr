#!/bin/bash
# Find "ProjectAuditorCI found X" line and exit with failure code if not X != 0
# Also print issue description and location

f () {
  local begin="$1"
  local OFFSET="$(expr ${2} \* 20)"
  local end="$3"
  local amount="$(expr ${2} \* 9 + 1)"
  local pipe=" | awk 'NR == $amount'"
  echo "${begin}${OFFSET}${end}${pipe}"
}

line="$(grep -E 'ProjectAuditorCI found *' analysis.log)"
number="$(cut -c 24-24 <<< ${line})"
issues="$(expr $number)"
echo "Static analysis found ${issues} code issues"
if [ $issues != 0 ] ; then
  for i in $(seq 1 $issues)
  do
    cmd=$(f "grep -E -A" "$i" " 'ProjectAuditorCI found *' analysis.log")
    eval $cmd
  done
  exit 1
fi
