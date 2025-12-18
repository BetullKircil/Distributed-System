FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["NotificationService.Worker/NotificationService.Worker.csproj", "NotificationService.Worker/"]
COPY ["Common.Shared/Common.Shared.csproj", "Common.Shared/"]

RUN dotnet restore "NotificationService.Worker/NotificationService.Worker.csproj"

COPY . .

WORKDIR "/src/NotificationService.Worker"
RUN dotnet build "NotificationService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Worker.dll"]