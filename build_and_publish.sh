#!/bin/bash
#
#	Generate and publish .NET binaries for project
#
#	Requirements:
#	- .NET SDK 8.0 or greater
#
#	Need to install .NET?  
#	
#	Microsoft's 'Install .NET on Linux' page can be found here:
#	https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website
#	
#	- zip
#	If zip isn't installed, you can use your Linux system package manager to install it.
#	( apt for Ubuntu/WSL, or dnf for Fedora/OpenSUSE )

echo " ***** CHECKING FOR .NET SDK AND ZIP *****"
echo ""

if ! command -v dotnet &> /dev/null; then
    echo "Error: `dotnet` command not found."
    echo "Please install .NET SDK 8.0 or greater."
    echo "Visit https://learn.microsoft.com/en-us/dotnet/core/install/linux for installation instructions."
    exit 1
fi

if ! command -v zip &> /dev/null; then
    echo "Error: `zip` command not found."
    echo "Please install zip using your distribution's package manager:"
    echo "  - For Ubuntu/Debian/WSL: sudo apt install zip"
    echo "  - For Fedora/RHEL: sudo dnf install zip"
    exit 1
fi

INSTALLED_VERSION=$(dotnet --version)
REQUIRED_VERSION="8.0"
if [[ "${INSTALLED_VERSION%%.*}" -lt "${REQUIRED_VERSION%%.*}" ]]; then
    echo "Error: .NET version $INSTALLED_VERSION is less than required version $REQUIRED_VERSION"
    echo "Please update your .NET SDK to version $REQUIRED_VERSION or greater."
    echo "Visit https://learn.microsoft.com/en-us/dotnet/core/install/linux for installation instructions."
    exit 1
fi

echo ""
echo "All required dependencies are installed. Proceeding with build..."
echo ""

PROJECT=./TrackerAppProject/TrackerApp/TrackerApp.csproj
STANDALONE_PROJECT=./TrackerAppProject/TrackerApp/TrackerApp-Standalone.csproj

# Output base directory
OUT_DIR="./published_binaries"
CROSS_DIR="./TrackerApp_CS690"
DLL_ZIP_FILE="./TrackerApp_CS690.zip"

# First -- build a cross-platform DLL
echo "Building cross-platform DLL..."

# Remove artifacts if they exist already
[ -d $CROSS_DIR ] && rm -rf $CROSS_DIR
[ -d $OUT_DIR ] && rm -rf $OUT_DIR

dotnet publish "$PROJECT" \
    -c Release \
    -o "$CROSS_DIR"

zip -q ./$DLL_ZIP_FILE $CROSS_DIR/*

mkdir -p $OUT_DIR

mv ./$DLL_ZIP_FILE ./$OUT_DIR/

#
# Make a zip file of the source directories for inclusion in alternate binary
# ( Project reviewer noted download didn't include source )
#
rm -rf ./TrackerAppProject/TrackerApp/bin ./TrackerAppProject/TrackerApp/obj
rm -rf ./TrackerAppProject/TrackerApp.Tests/bin ./TrackerAppProject/TrackerApp.Tests/obj
#zip -q TrackerAppProject_Source.zip TrackerAppProject/*
zip -q TrackerAppProject_CS690_DLL_and_Source.zip $CROSS_DIR/* TrackerAppProject/*
mv ./TrackerAppProject_CS690_DLL_and_Source.zip ./$OUT_DIR/

[ -d $CROSS_DIR ] && rm -rf $CROSS_DIR

echo "Cross-platfrom DLL built, TrackerApp_CS690.zip moved to ${OUT_DIR} along with Source-Included zipfile"
echo ""

# Second:   if we're going to build standalone binaries ( with pdb ), 
#           let's build ALL the platforms
#
# Runtime Identifiers (RIDs)

cp ./Utils/TrackerApp-Standalone.csproj ./TrackerAppProject/TrackerApp/

RIDS=("win-x64" "linux-x64" "osx-arm64" "osx-x64" )
echo "Building TrackerApp-Standalone for each of the following: ${RIDS[@]}"
echo ""

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

    echo "Done: $OUT_DIR/$RID has been packaged in a zip file in $OUT_DIR"
    echo ""
done

rm ./TrackerAppProject/TrackerApp/TrackerApp-Standalone.csproj 

echo "All targets published."

