#!/bin/bash

PROJECT=TrackerAppProject/TrackerApp/TrackerApp.csproj

DOTNET_VERSION="net8.0"

# Output base directory
OUT_DIR="./published_binaries"
CROSS_DIR="$OUT_DIR/cross-platform"

# First -- build a cross-platform DLL
echo "Building cross-platform DLL..."

dotnet publish "$PROJECT" \
    -c Release \
    -o "$CROSS_DIR"

#
# Second:   if we're going to build standalone binaries ( with pdb ), 
#           let's build ALL the platforms
#
# Runtime Identifiers (RIDs)
RIDS=("win-x64" "linux-x64" "osx-arm64" "osx-x64" )

for RID in "${RIDS[@]}"; do
    echo "Publishing for $RID..."

    dotnet publish "$PROJECT" \
        -c Release \
        -r "$RID" \
        --self-contained true \
        -p:PublishSingleFile=true \
        -p:IncludeNativeLibrariesForSelfExtract=true \
        -o "$OUT_DIR/$RID"

    echo "Done: $OUT_DIR/$RID"
    echo ""
done

echo "All targets published."

