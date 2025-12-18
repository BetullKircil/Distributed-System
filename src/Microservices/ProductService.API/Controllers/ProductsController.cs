using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.API.Data;
using ProductService.API.Models;

namespace ProductService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductDbContext _context;
    private readonly IDistributedCache _cache;

    public ProductsController(ProductDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        string cacheKey = $"product_{id}";

        string? cachedProduct = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedProduct))
        {
            var productFromCache = JsonSerializer.Deserialize<Product>(cachedProduct);
            return Ok(productFromCache);
        }

        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        var productToCache = JsonSerializer.Serialize(product);
        await _cache.SetStringAsync(cacheKey, productToCache, cacheOptions);

        return Ok(product);
    }
}