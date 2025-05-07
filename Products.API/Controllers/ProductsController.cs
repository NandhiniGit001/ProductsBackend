using Microsoft.AspNetCore.Mvc;
using Products.API.DTO;
using Products.API.Services;

namespace Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(
          [FromQuery] string? sort = null,
          [FromQuery] bool? expansiveThan2PerLiter = null)
        {
            try
            {
                if (sort != null && sort.ToLower() != "asc" && sort.ToLower() != "desc")
                {
                    _logger.LogWarning("Invalid sort parameter received: {Sort}", sort);
                    return BadRequest(new
                    {
                        message = "Invalid sort parameter. Allowed values are 'asc' or 'desc'."
                    });
                }

                var products = await _productService.GetAllArticlesAsync(sort, expansiveThan2PerLiter ?? false);

                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found");
                    return NotFound(new { message = "No articles found." });
                }

                _logger.LogInformation("Products return successfully");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting articles.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An internal server error occurred.",
                    details = ex.Message
                });
            }
        }
    }
}
