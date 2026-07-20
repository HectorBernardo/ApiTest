
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app


COPY *.sln .
COPY src/ApiTest.Api/*.csproj ./src/ApiTest.Api/
COPY src/ApiTest.Application/*.csproj ./src/ApiTest.Application/
COPY src/ApiTest.Domain/*.csproj ./src/ApiTest.Domain/
COPY src/ApiTest.Infrastructure/*.csproj ./src/ApiTest.Infrastructure/
RUN dotnet restore


COPY . .
RUN dotnet publish src/ApiTest.Api/ApiTest.Api.csproj -c Release -o /out


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "ApiTest.Api.dll"]