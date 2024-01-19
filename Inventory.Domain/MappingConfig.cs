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
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Data.Entities.Inventory, InventoryModel>().ReverseMap();
        }
    }
}
