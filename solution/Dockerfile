FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# Copiar solución y proyecto, y descargar NuGets
COPY *.sln .
COPY src/Counter.Web/*.csproj ./src/Counter.Web/
COPY test/Counter.Web.Test/*.csproj ./test/Counter.Web.Test/
RUN dotnet restore

# Copiar los fuentes del proyecto
COPY src/Counter.Web/. ./src/Counter.Web/
COPY test/Counter.Web.Test/. ./test/Counter.Web.Test/
# Generar la aplicación
WORKDIR /app/src/Counter.Web
RUN dotnet publish -c Release
# Ejecutar pruebas unitarias
WORKDIR /app
RUN dotnet test ./test/Counter.Web.Test/Counter.Web.Test.csproj

# Copiar la aplicación generada a su imagen final
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/src/Counter.Web/publish/netcoreapp2.2/publish ./
ENTRYPOINT ["dotnet", "Counter.Web.dll"]
