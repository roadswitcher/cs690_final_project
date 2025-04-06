#!/bin/bash

set -x

PROJECT=TrackerAppProject/TrackerApp/TrackerApp.csproj
STANDALONE_PROJECT=TrackerAppProject/TrackerApp/TrackerApp-Standalone.csproj

DOTNET_VERSION="net8.0"

# Output base directory
OUT_DIR="./published_binaries"
CROSS_DIR="./TrackerApp_CS690"
DLL_ZIP_FILE="TrackerApp_CS690.zip"

# First -- build a cross-platform DLL
echo "Building cross-platform DLL..."

# Remove artifacts if they exist already
[ -d $CROSS_DIR ] && rm -rf $CROSS_DIR
[ -d $OUT_DIR ] && rm -rf $OUT_DIR

dotnet publish "$PROJECT" \
    -c Release \
    -o "$CROSS_DIR"

zip ./$DLL_ZIP_FILE $CROSS_DIR/*

mkdir -p $OUT_DIR

mv ./$DLL_ZIP_FILE ./$OUT_DIR/
[ -d $CROSS_DIR ] && rm -rf $CROSS_DIR


#
# Second:   if we're going to build standalone binaries ( with pdb ), 
#           let's build ALL the platforms
#
# Runtime Identifiers (RIDs)
RIDS=("win-x64" "linux-x64" "osx-arm64" "osx-x64" )

for RID in "${RIDS[@]}"; do
    echo "Publishing for $RID..."

    # Getting this to work was interesting -- I learned you couldn't generate standalone exes
    # with the same csproj as the multiplatform DLL.  
    # 
    # NOTE:   If you get warnings about trimmed binaries and JSON serialization you CANNOT
    #         trim your binaries, it'll break the reflection things like Spectre Console and JSON depend on
    dotnet publish "$STANDALONE_PROJECT" \
        -c Release \
        -r "$RID" \
        --self-contained true \
        -p:PublishSingleFile=true \
        -p:IncludeNativeLibrariesForSelfExtract=true \
        -o "$OUT_DIR/$RID"

    cd $OUT_DIR
    zip "TrackerApp-${RID}.zip" $RID/*
    cd ..

    echo "Done: $OUT_DIR/$RID"
    echo ""
done


echo "All targets published."

