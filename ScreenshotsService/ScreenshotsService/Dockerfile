FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 51256
EXPOSE 44377

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ScreenshotsService/ScreenshotsService.csproj ScreenshotsService/
RUN dotnet restore ScreenshotsService/ScreenshotsService.csproj
COPY . .
WORKDIR /src/ScreenshotsService
RUN dotnet build ScreenshotsService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ScreenshotsService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ScreenshotsService.dll"]
