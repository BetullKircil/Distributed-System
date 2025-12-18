using IdentityService.API.Data;
using IdentityService.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(options => { options.EmitStaticAudienceClaim = true; })
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryApiScopes(new List<ApiScope>
    {
        new ApiScope("product_api", "Product API"),
        new ApiScope("customer_api", "Customer API"),
        new ApiScope("order_api", "Order API")
    })
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "gateway_client",
            // Şifre: "secret" (SHA256 hashlenmiş hali)
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "product_api", "customer_api", "order_api" }
        }
    });

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
}

app.UseIdentityServer(); 
app.MapControllers();

app.Run();