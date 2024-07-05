using Application.Common.Mapping;
using Application.Common.Mapping.Resolver;
using Application.Dtos.Common;
using AutoMapper;
using Domain.Entities.ProductEntity;

namespace Application.Dtos.Products
{
    public class ProductDto:CommandDto,IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public int ProductTypeId { get; set; }
        //
        public int ProductBrandId { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>()
                .ForMember(x=>x.PictureUrl,c=>c.MapFrom<ProductImageUrlResolver>())
                .ForMember(x=>x.ProductBrand,c=>c.MapFrom(v=>v.ProductBrand.Title))
                .ForMember(x => x.ProductType, c => c.MapFrom(v => v.ProductType.Title));
        }
    }
}
