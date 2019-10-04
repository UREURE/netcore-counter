FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# Copiar solución y proyecto, y descargar NuGets
COPY *.sln .
COPY src/Counter.Web/*.csproj ./src/Counter.Web/
RUN dotnet restore

# Copiar los fuentes del proyecto y compilar
COPY src/Counter.Web/. ./src/Counter.Web/
WORKDIR /app/src/Counter.Web
RUN dotnet publish -c Release

# Copiar la aplicación generada a su imagen final
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/src/Counter.Web/publish/netcoreapp2.2/publish ./
ENTRYPOINT ["dotnet", "Counter.Web.dll"]
