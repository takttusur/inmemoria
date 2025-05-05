FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaktTusur.InMemoria.DataAccess.Migrations/TaktTusur.InMemoria.DataAccess.Migrations.csproj", "TaktTusur.InMemoria.DataAccess.Migrations/"]
RUN dotnet restore "TaktTusur.InMemoria.DataAccess.Migrations/TaktTusur.InMemoria.DataAccess.Migrations.csproj"
COPY . .
WORKDIR "/src/TaktTusur.InMemoria.DataAccess.Migrations"
RUN dotnet publish "TaktTusur.InMemoria.DataAccess.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish -p:RunMigration=true /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
ARG APP_UID=1000
WORKDIR /app
COPY --from=build /app/publish .
USER ${APP_UID}
ENTRYPOINT ["dotnet", "TaktTusur.InMemoria.DataAccess.Migrations.dll"]
