#!/bin/bash

# Your project name (or path to the .csproj file)
PROJECT=$1

# Target .NET runtime version
DOTNET_VERSION="net8.0"

# Output base directory
OUT_DIR="./publish"

# Runtime Identifiers (RIDs)
RIDS=("win-x64" "osx-x64" "linux-x64")

for RID in "${RIDS[@]}"; do
    echo "Publishing for $RID..."

    dotnet publish "$PROJECT" \
        -c Release \
        -r "$RID" \
        --self-contained true \
        -p:PublishSingleFile=true \
        -p:IncludeNativeLibrariesForSelfExtract=true \
        -p:PublishTrimmed=true \
        -o "$OUT_DIR/$RID"

    echo "Done: $OUT_DIR/$RID"
    echo ""
done

echo "All targets published."

