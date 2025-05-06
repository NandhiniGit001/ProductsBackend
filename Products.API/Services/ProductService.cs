using AutoMapper;
using Microsoft.Extensions.Logging;
using Products.API.DTO;
using Products.API.Models;
using Products.API.Repository;
using System.Globalization;

namespace Products.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ArticleDto>> GetAllArticlesAsync(string? sortOrder, bool filterExpensive)
        {
            var products = await _productRepository.GetAllProductsAsync();

            var allArticles = products.SelectMany(p => p.Articles
                .Where(a => !filterExpensive || GetPricePerLiter(a) >= 2.0)
                .Select(a =>
                {
                    var dto = _mapper.Map<ArticleDto>(a);
                    dto.ProductId = p.Id;
                    dto.ProductName = p.Name;
                    return dto;
                }))
                .ToList();

            _logger.LogInformation("Prepared {Count} articles after filtering.", allArticles.Count);

            if (sortOrder?.ToLower() is "asc" or "desc")
            {
                allArticles = sortOrder == "asc"
                    ? allArticles.OrderBy(a => a.Price).ToList()
                    : allArticles.OrderByDescending(a => a.Price).ToList();
            }

            _logger.LogInformation("Sorted articles by price in {Order} order.", sortOrder);
            return allArticles;
        }

        private double GetPricePerLiter(Article article)
        {
            var text = article.PricePerUnitText.Replace("€", "").Replace(",", ".").Replace("/Liter", "").Replace("(", "").Replace(")", "").Trim();
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            _logger.LogWarning("Failed to parse price per liter for article ID {Id}, using default value.", article.Id);
            return double.MaxValue;
        }
    }
}
