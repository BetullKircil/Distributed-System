using Microsoft.EntityFrameworkCore;
using ProductService.API.Data;
using ProductService.API.GraphQl.Query;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<ProductQuery>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
}

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis:6379"; 
    options.InstanceName = "ProductService_"; 
});

app.MapControllers();
app.MapGraphQL();

app.Run();