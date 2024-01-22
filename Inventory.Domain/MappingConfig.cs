using AutoMapper;
using Inventory.Data.Entities;
using Inventory.Data.Models;

namespace Inventory.Domain
{
    internal class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Purchase, PurchaseModel>().ReverseMap();
            CreateMap<Category, CategoryModel>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(opt => opt.OrganizationId));
            CreateMap<CategoryModel, Category>()
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(opt => opt.BusinessId))
                .ForMember(dest => dest.Products, opt => opt.Ignore());
            CreateMap<ProductModel, Product>().ReverseMap();
            CreateMap<Data.Entities.Inventory, InventoryModel>().ReverseMap();
        }
    }
}
