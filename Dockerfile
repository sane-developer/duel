### Stage 1: Build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /build

COPY src/ ./src/

COPY tests/ ./tests/

RUN dotnet restore ./src/Duel.sln

RUN dotnet build ./src/Duel.API/Duel.API.csproj -c Release

RUN dotnet publish ./src/Duel.API/Duel.API.csproj -c Release --no-build -o /app/publish

### Stage 2: Runtime

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

RUN groupadd -r -g 1000 duel-app && useradd -r -u 1000 -g duel-app duel-app

COPY --from=build /app/publish ./

RUN chown -R duel-app:duel-app /app

USER duel-app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENV ASPNETCORE_ENVIRONMENT=Production

ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "Duel.API.dll"]