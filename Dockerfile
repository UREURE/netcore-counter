FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# Copiar solución y proyecto, y descargar NuGets
COPY *.sln .
COPY Counter.Web/*.csproj ./Counter.Web/
RUN dotnet restore

# Copiar los fuentes del proyecto y compilar
COPY Counter.Web/. ./Counter.Web/
WORKDIR /app/Counter.Web
RUN dotnet publish -c Release

# Copiar la aplicación generada a su imagen final
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/Counter.Web/publish/netcoreapp2.2/publish ./
ENTRYPOINT ["dotnet", "Counter.Web.dll"]
