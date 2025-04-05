#!/usr/bin/env bash
set -x
dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true /p:UseAppHost=true

# Define the path to the publish binary 
PUBLISH_DIR="./TrackerApp/bin/Release/net8.0/linux-x64/publish/"

# Define the name of the binary
BINARY_NAME="TrackerApp"

# Construct the full path to the binary
BINARY_PATH="$PUBLISH_DIR/$BINARY_NAME"

# Check if the binary exists
if [ -f "$BINARY_PATH" ]; then
    # Copy the binary to the current working directory
    mkdir -p ./bin/
    cp "$BINARY_PATH" ./bin/
    echo "Single-file binary for TrackerApp has been copied to the ./bin subdirectory"
else
    echo "Error: Binary not found in the publish directory. Make sure you have run dotnet publish first."
fi

