FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaktTusur.InMemoria.Domain/TaktTusur.InMemoria.Domain.csproj", "TaktTusur.InMemoria.Domain/"]
COPY ["TaktTusur.InMemoria.MigrationApp/TaktTusur.InMemoria.MigrationApp.csproj", "TaktTusur.InMemoria.MigrationApp/"]
RUN dotnet restore "TaktTusur.InMemoria.Domain/TaktTusur.InMemoria.Domain.csproj"
RUN dotnet restore "TaktTusur.InMemoria.MigrationApp/TaktTusur.InMemoria.MigrationApp.csproj"
COPY . .
WORKDIR "/src/TaktTusur.InMemoria.MigrationApp"
RUN dotnet build "TaktTusur.InMemoria.MigrationApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaktTusur.InMemoria.MigrationApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaktTusur.InMemoria.MigrationApp.dll"]
