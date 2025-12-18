# 1. Aşama: Build (İnşaat)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala (Restore işlemi için)
# NOT: Common.Shared projesi olduğu için onu da kopyalamalıyız!
COPY ["OrderService.API/OrderService.API.csproj", "OrderService.API/"]
COPY ["Common.Shared/Common.Shared.csproj", "Common.Shared/"]

# Bağımlılıkları yükle
RUN dotnet restore "OrderService.API/OrderService.API.csproj"

# Tüm kodları kopyala
COPY . .

# Build al
WORKDIR "/src/OrderService.API"
RUN dotnet build "OrderService.API.csproj" -c Release -o /app/build

# Publish al (Yayına hazır hale getir)
FROM build AS publish
RUN dotnet publish "OrderService.API.csproj" -c Release -o /app/publish

# 2. Aşama: Runtime (Çalıştırma)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]