FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY . .
RUN dotnet restore NosCore.ParserInputGenerator.sln
RUN dotnet publish src/NosCore.ParserInputGenerator.Launcher/NosCore.ParserInputGenerator.Launcher.csproj \
    --configuration Release --no-restore --output /out

FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs
WORKDIR /app/build/net10.0
COPY --from=build /out/ ./
COPY configuration /app/configuration
ENTRYPOINT ["dotnet", "NosCore.ParserInputGenerator.Launcher.dll"]
