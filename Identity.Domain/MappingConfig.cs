using AutoMapper;
using Identity.Data.Entities;
using Identity.Data.Models;

namespace Inventory.Domain
{
    internal class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Application, ApplicationInfoModel>().ReverseMap();
            CreateMap<Organization, OrganizationModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
        }
    }
}
