using Products.API.Models;

namespace Products.API.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}
