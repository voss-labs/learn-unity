#!/usr/bin/env bash
set -euo pipefail

root="$(cd "$(dirname "$0")/.." && pwd)"
worlds=("$@")
[ ${#worlds[@]} -gt 0 ] || worlds=(Starter Campus Home)

for world in "${worlds[@]}"; do
  rm -rf "$root/site/$(echo "$world" | tr '[:upper:]' '[:lower:]')"
  unity build "$root" \
    --target WebGL \
    --execute-method WebBuild.Run \
    --args "-world $world" \
    --allow-dirty-build \
    --no-provenance \
    --no-tail \
    --timeout 3600 \
    --format json
done
