using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Products.API.Models;

namespace Products.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private const string ExternalDataUrl = "https://flapotest.blob.core.windows.net/test/ProductData.json";
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(HttpClient httpClient, ILogger<ProductRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                var rawJson = await _httpClient.GetStringAsync(ExternalDataUrl);
                _logger.LogInformation("Successfully fetched product data.");
                var products = JsonConvert.DeserializeObject<List<Product>>(rawJson) ?? new();
                _logger.LogInformation("Deserialized {Count} products.", products.Count);
                return products;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products from external URL.");
                throw;
            }
        }
    }
}
