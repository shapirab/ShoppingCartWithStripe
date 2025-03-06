using AutoMapper;
using ShoppingCart.data.DataModels.Dtos;
using ShoppingCart.data.DataModels.Dtos.OrderDtos;
using ShoppingCart.data.DataModels.Dtos.UserDtos;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
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
            
            CreateMap<ProductEntity, ProductToReturnDto>()
                .ForMember(destination => 
                    destination.ProductBrand, origin => origin.MapFrom(sourceMember => sourceMember.ProductBrand.Name))
                .ForMember(destination => 
                    destination.ProductType, origin => origin.MapFrom(sourceMember => sourceMember.ProductType.Name));

            CreateMap<ProductBrand, ProductBrandEntity>();
            CreateMap<ProductBrandEntity, ProductBrand>();
            CreateMap<ProductBrandDto, ProductBrandEntity>();
            CreateMap<ProductBrandEntity, ProductBrandDto>();

            CreateMap<ProductType, ProductTypeEntity>();
            CreateMap<ProductTypeEntity, ProductType>();
            CreateMap<ProductTypeDto, ProductTypeEntity>();
            CreateMap<ProductTypeEntity, ProductTypeDto>();

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<AppUser, RegisterDto>();
            CreateMap<RegisterDto, AppUser>();

            CreateMap<Order, ReturnOrderDto>();
            CreateMap<ReturnOrderDto, Order>();

        }
    }
}
