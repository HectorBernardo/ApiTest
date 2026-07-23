FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY *.sln .
COPY ApiTest.Web/*.csproj ./ApiTest.Web/
RUN dotnet restore "ApiTest.Web/ApiTest.Web.csproj"

COPY . .
WORKDIR /app/ApiTest.Web
RUN dotnet publish -c Release -o /out


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "ApiTest.Web.dll"]