
using Xunit;
using Moq;
using AutoMapper;
using Products.API.Services;
using Products.API.Repository;
using Products.API.Models;
using Products.API.DTO;
using Microsoft.Extensions.Logging;
using FluentAssertions;


namespace Products.Test
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo = new();
        private readonly IMapper _mapper;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Article, ArticleDto>();
            });
            _mapper = config.CreateMapper();

            var logger = new Mock<ILogger<ProductService>>();
            _service = new ProductService(_mockRepo.Object, _mapper, logger.Object);
        }

        [Fact]
        public async Task Returns_All_Articles_When_No_Filter_Or_Sort()
        {
            // Arrange
            var product = CreateProduct(1, "Pils", 1.5, "(1,50 €/Liter)");
            _mockRepo.Setup(r => r.GetAllProductsAsync())
                     .ReturnsAsync(new List<Product> { product });

            // Act
            var result = await _service.GetAllArticlesAsync(null, false);

            // Assert
            result.Should().HaveCount(1);
            result[0].ProductId.Should().Be(1);
            result[0].Price.Should().Be(1.5);
        }

        [Fact]
        public async Task Filters_Out_Beers_More_Than_2_Euro_Per_Liter()
        {
            var product = CreateProduct(1, "Strong", 3.99, "(3,99 €/Liter)");
            _mockRepo.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(new List<Product> { product });

            var result = await _service.GetAllArticlesAsync(null, true);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Sorts_By_Price_Ascending()
        {
            var products = new List<Product>
        {
            CreateProduct(1, "B", 5.0, "(1,50 €/Liter)"),
            CreateProduct(2, "A", 2.0, "(1,00 €/Liter)")
        };
            _mockRepo.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(products);

            var result = await _service.GetAllArticlesAsync("asc", false);

            result.Select(r => r.Price).Should().ContainInOrder(2.0, 5.0);
        }

        [Fact]
        public async Task Sorts_By_Price_Descending()
        {
            var products = new List<Product>
        {
            CreateProduct(1, "A", 1.0, "(1,00 €/Liter)"),
            CreateProduct(2, "B", 3.0, "(1,00 €/Liter)")
        };
            _mockRepo.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(products);

            var result = await _service.GetAllArticlesAsync("desc", false);

            result.Select(r => r.Price).Should().ContainInOrder(3.0, 1.0);
        }

        [Fact]
        public async Task Ignores_Articles_With_Invalid_PricePerUnitText()
        {
            var product = new Product
            {
                Id = 10,
                Name = "Broken",
                BrandName = "X",
                Articles = new List<Article>
            {
                new() {
                    Id = 100, Price = 10.0, ShortDescription = "?", Unit = "Liter",
                    Image = "img", PricePerUnitText = "invalid"
                }
            }
            };

            _mockRepo.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(new List<Product> { product });

            var result = await _service.GetAllArticlesAsync(null, true);

            result.Should().BeEmpty();
        }

        private Product CreateProduct(int id, string name, double price, string pricePerUnit)
        {
            return new Product
            {
                Id = id,
                Name = name,
                BrandName = "Test",
                Articles = new List<Article>
            {
                new Article
                {
                    Id = id * 10,
                    ShortDescription = $"{name} desc",
                    Price = price,
                    Unit = "Liter",
                    PricePerUnitText = pricePerUnit,
                    Image = "img"
                }
            }
            };
        }
    }
}