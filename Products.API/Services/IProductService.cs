using Products.API.DTO;

namespace Products.API.Services
{
    public interface IProductService
    {
        Task<List<ArticleDto>> GetAllArticlesAsync(string? sortOrder, bool filterExpensive);
    }
}
