FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["CustomerService.API/CustomerService.API.csproj", "CustomerService.API/"]

RUN dotnet restore "CustomerService.API/CustomerService.API.csproj"

COPY . .

WORKDIR "/src/CustomerService.API"
RUN dotnet build "CustomerService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.API.dll"]