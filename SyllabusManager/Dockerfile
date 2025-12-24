# 1️⃣ Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# csproj'yi önce kopyala (cache için önemli)
COPY *.csproj ./
RUN dotnet restore

# tüm kaynak kodu kopyala
COPY . ./
RUN dotnet publish -c Release -o /out

# 2️⃣ Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /out ./

ENTRYPOINT ["dotnet", "SyllabusManager.dll"]