using ProductService.API.Data;
using ProductService.API.Models;

namespace ProductService.API.GraphQl.Query;

public class ProductQuery
{
    public async Task<IEnumerable<Product>> GetProducts([Service] ProductRepository repository)
    {
        return await repository.GetAllAsync();
    }
}