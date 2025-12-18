using Dapper;
using Npgsql;
using ProductService.API.Models;

namespace ProductService.API.Data;

public class ProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Product>("SELECT * FROM \"Products\"");
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Product>(
            "SELECT * FROM \"Products\" WHERE \"Id\" = @Id", new { Id = id });
    }
}