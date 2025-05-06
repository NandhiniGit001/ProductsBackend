using AutoMapper;
using Products.API.DTO;
using Products.API.Models;

namespace Products.API.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductName, opt => opt.Ignore());
        }

    }
}
