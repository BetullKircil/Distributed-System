using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Data;

var builder = WebApplication.CreateBuilder(args);

// --- LOGLAMA ---
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

Console.WriteLine("--> [Startup] Servis baslatiliyor...");

// 1. EF Core (Veritabanı)
// Docker'da "ConnectionStrings__DefaultConnection" env variable'ı burayı ezecek.
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    Console.WriteLine("--> [Startup] DB Provider ayarlaniyor...");
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 2. MassTransit & RabbitMQ (Mesajlaşma)
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // Docker içindeki rabbitmq servisine bağlanıyoruz
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// --- 3. OTOMATİK MIGRATION (Tablo Oluşturma) ---
// Burasi cok onemli! Uygulama ayaga kalkarken veritabanini olusturur.
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        
        Console.WriteLine("--> [Startup] Veritabani migrasyonlari uygulaniyor...");
        // Eger onceden migration dosyasi olusturmadiysan EnsureCreated kullanabilirsin (Test icin)
        // dbContext.Database.Migrate(); // Production icin dogrusu budur ama Migration dosyasi ister.
        dbContext.Database.EnsureCreated(); // Test icin hizli cozum: Tablolari direkt olusturur.
        
        Console.WriteLine("--> [Startup] Veritabani hazir!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"--> [FATAL ERROR] Veritabani olusturulurken hata: {ex.Message}");
    // Hata varsa burada loglarda gorecegiz.
}

app.MapControllers();

Console.WriteLine("--> [Startup] Uygulama calisiyor, istekler bekleniyor...");
app.Run();