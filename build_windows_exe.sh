#!/bin/bash

# Define output directory
OUTPUT_DIR="./windows_build"

echo "🚀 Starting Windows Build via Docker..."
echo "Target: Windows x64 (Self-Contained, Single File)"

# Run dotnet publish inside a Docker container
# -v: Mounts current directory to /source
# -w: Sets working directory
# Image: mcr.microsoft.com/dotnet/sdk:8.0 (Official .NET 8 SDK)
docker run --rm -v "$(pwd):/source" -w /source mcr.microsoft.com/dotnet/sdk:8.0 \
    dotnet publish SyllabusManager/SyllabusManager.csproj \
    -c Release \
    -r win-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:DebugType=None \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o $OUTPUT_DIR

echo "✅ Build Process Completed."
echo "📂 Check the '$OUTPUT_DIR' folder for your 'SyllabusManager.exe' file."
