FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LgConnectWs.Test/LgConnectWs.Test.csproj", "LgConnectWs.Test/"]
RUN dotnet restore "LgConnectWs.Test/LgConnectWs.Test.csproj"
COPY . .
WORKDIR "/src/LgConnectWs.Test"
RUN dotnet build "LgConnectWs.Test.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LgConnectWs.Test.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LgConnectWs.Test.dll"]
