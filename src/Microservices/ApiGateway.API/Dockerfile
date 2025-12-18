FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ApiGateway.API/ApiGateway.API.csproj", "ApiGateway.API/"]

RUN dotnet restore "ApiGateway.API/ApiGateway.API.csproj"

COPY . .

WORKDIR "/src/ApiGateway.API"
RUN dotnet build "ApiGateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGateway.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.API.dll"]