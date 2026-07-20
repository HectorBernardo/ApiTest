# Etapa de compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiar todo el código de la solución al contenedor
COPY . .

# Restaurar y compilar directamente apuntando al proyecto API
WORKDIR /app/src/ApiTest.Api
RUN dotnet publish -c Release -o /app/out

# Etapa de ejecución (Runtime más liviano)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ApiTest.Api.dll"]