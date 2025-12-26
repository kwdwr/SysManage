# 1️⃣ Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files explicitly (assuming build context is root 'SysManage')
COPY ["SyllabusManager/SyllabusManager.csproj", "SyllabusManager/"]
COPY ["Tests/Tests.csproj", "Tests/"]
RUN dotnet restore "SyllabusManager/SyllabusManager.csproj"
RUN dotnet restore "Tests/Tests.csproj"

# Copy all source
COPY . .

# Build
WORKDIR "/src/SyllabusManager"
RUN dotnet build -c Release -o /app/build

# Run Tests
WORKDIR "/src/Tests"
RUN dotnet test -c Release --verbosity normal

# Publish
WORKDIR "/src/SyllabusManager"
RUN dotnet publish -c Release -o /app/publish

# 2️⃣ Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SyllabusManager.dll"]