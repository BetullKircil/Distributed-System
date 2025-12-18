using CustomerService.API.Models;
using Dapper;
using Npgsql;

namespace CustomerService.API.Data;

public class CustomerRepository
{
    private readonly IConfiguration _configuration;

    public CustomerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Customer>("SELECT * FROM \"Customers\"");
    }
}