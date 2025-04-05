# On a Windows platform?   Run this, it should work.

# If your Windows install is configured to block Powershell executation by default,
# the following command MIGHT enable temporary execution of Powershell scripts in the
# current Powershell session 
#
# Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
#
# If that doesn't work, just copy the binary manually

# Build a single file binary for Windows
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:UseAppHost=true
 
# Define the default path to the publish directory
$publishDir = ".\TrackerApp\bin\Release\net8.0\win-x64\publish"

# Construct the path to the single-file binary
$binaryPath = Join-Path $publishDir "TrackerApp.exe"

# Copy the binary to the current working directory
Copy-Item $binaryPath $PWD

Write-Host "Single-file binary 'TrackerApp.exe' has been copied to the current working directory."

