using AutoMapper;
using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Data.Models.Product;
using Inventory.Data.Models.Purchase;

namespace Inventory.Domain
{
    internal class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<PurchaseInvoice, PurchaseInvoiceModel>()
                .ForMember(dest => dest.PurchaseDetails, src => src.MapFrom(opt => opt.Purchases))
                .ReverseMap();

            CreateMap<PurchaseInvoiceDetails, InvoiceDetailsModel>()
                .ForMember(dest => dest.PurchasePrice, src => src.MapFrom(opt => opt.PurchasePrice))
                .ReverseMap();

            CreateMap<Category, CategoryModel>()
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(opt => opt.OrganizationId))
                .ReverseMap()
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(opt => opt.OrganizationId))
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());

            CreateMap<ProductModel, Product>()
                .ForMember(dest => dest.QuantityAttributeId, opt => opt.MapFrom(opt => opt.QuantityTypeId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(opt => opt.SubcategoryId))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(opt => opt.BrandAttributeId))
                .ReverseMap()
                .ForMember(dest => dest.QuantityTypeId, opt => opt.MapFrom(opt => opt.QuantityAttributeId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(opt => opt.Category.ParentCategory))
                .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(opt => opt.CategoryId))
                .ForMember(dest => dest.PurchasePrice, opt => opt.Ignore())
                .ForMember(dest => dest.RetailPrice, opt => opt.Ignore());

            CreateMap<Vendor, VendorModel>().ReverseMap();

            CreateMap<CommonAttribute, CommonAttributeModel>()
                .ReverseMap();
                //.ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}
