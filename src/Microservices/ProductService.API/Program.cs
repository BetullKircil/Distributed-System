using Microsoft.EntityFrameworkCore;
using ProductService.API.Data;
using ProductService.API.GraphQl.Query;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductRepository>();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis:6379";
    options.InstanceName = "ProductService_";
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<ProductQuery>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ProductDbContext>();
        db.Database.Migrate(); 
        Console.WriteLine("--> ProductService veritabanı güncellendi/oluşturuldu.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Veritabanı migrasyon hatası: {ex.Message}");
    }
}

app.MapControllers();
app.MapGraphQL();

app.Run();