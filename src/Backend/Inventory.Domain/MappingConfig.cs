using AutoMapper;
using Inventory.Data.Entities;
using Inventory.Data.Models;
using Inventory.Data.Models.CRM;
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

            CreateMap<PurchaseInvoiceDetails, PurchaseInvoiceDetailsModel>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(opt => opt.Quantity))
                .ReverseMap();

            CreateMap<Category, CategoryModel>()
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(opt => opt.OrganizationId))
                .ReverseMap()
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(opt => opt.OrganizationId))
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());

            CreateMap<ProductVariantModel, ProductVariant>()
                .ForMember(dest => dest.BarCode, opt => opt.MapFrom(opt => opt.BarCode))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(opt => opt.SKU))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(opt => opt.Attributes))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(opt => opt.StockQuantity))
                .ForMember(dest => dest.CreateBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductModel, Product>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(opt => opt.SubcategoryId))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(opt => opt.BrandId))
                .ForMember(dest => dest.QuantityAttributeId, opt => opt.MapFrom(opt => opt.QuantityTypeId))
                .ReverseMap()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(opt => opt.Category.ParentCategory))
                .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(opt => opt.CategoryId))
                .ForMember(dest => dest.PurchasePrice, opt => opt.Ignore())
                .ForMember(dest => dest.RetailPrice, opt => opt.Ignore());

            CreateMap<Vendor, VendorModel>().ReverseMap();

            CreateMap<CommonAttribute, CommonAttributeModel>()
                .ReverseMap();

            CreateMap<Customer, CustomerModel>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.ContactNumber, opt => opt.MapFrom(src => src.ContactNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<CustomerDetails, CustomerModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                .ReverseMap();
        }
    }
}
