#!/bin/bash

set -x

PROJECT=TrackerAppProject/TrackerApp/TrackerApp.csproj

DOTNET_VERSION="net8.0"

# Output base directory
OUT_DIR="./published_binaries"
CROSS_DIR="./TrackerApp_CS690"
DLL_ZIP_FILE="TrackerApp_CS690.zip"

# First -- build a cross-platform DLL
echo "Building cross-platform DLL..."

# Remove artifacts if they exist already
[ -d $CROSS_DIR ] && rm -rf $CROSS_DIR
[ -r $DLL_ZIP_FILE ] && rm -rf $DLL_ZIP_FILE

dotnet publish "$PROJECT" \
    -c Release \
    -o "$CROSS_DIR"

cd $CROSS_DIR
zip ../$DLL_ZIP_FILE ./*
cd ..

#
# Second:   if we're going to build standalone binaries ( with pdb ), 
#           let's build ALL the platforms
#
# Runtime Identifiers (RIDs)
#RIDS=("win-x64" "linux-x64" "osx-arm64" "osx-x64" )
#
#for RID in "${RIDS[@]}"; do
#    echo "Publishing for $RID..."
#
#    dotnet publish "$PROJECT" \
#        -c Release \
#        -r "$RID" \
#        --self-contained true \
#        -p:PublishSingleFile=true \
#        -p:IncludeNativeLibrariesForSelfExtract=true \
#        -o "$OUT_DIR/$RID"
#
#    echo "Done: $OUT_DIR/$RID"
#    echo ""
#done

echo "All targets published."

