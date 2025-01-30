using AutoMapper;
using ShoppingCart.data.DataModels.Dtos;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models;

namespace ShoppingCart.api.Profiles
{
    public class AppProfiles : Profile
    {
        public AppProfiles()
        {
            CreateMap<Product, ProductEntity>();
            CreateMap<ProductEntity, Product>();
            CreateMap<ProductDto, ProductEntity>();
            CreateMap<ProductEntity, ProductDto>();
            
            CreateMap<ProductBrand, ProductBrandEntity>();
            CreateMap<ProductBrandEntity, ProductBrand>();
            CreateMap<ProductBrandDto, ProductBrandEntity>();
            CreateMap<ProductBrandEntity, ProductBrandDto>();

            CreateMap<ProductType, ProductTypeEntity>();
            CreateMap<ProductTypeEntity, ProductType>();
            CreateMap<ProductTypeDto, ProductTypeEntity>();
            CreateMap<ProductTypeEntity, ProductTypeDto>();
        }
    }
}
