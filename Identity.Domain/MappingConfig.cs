using AutoMapper;
using Identity.Data.Entities;
using Identity.Data.Models;

namespace Inventory.Domain
{
    internal class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Application, ApplicationInfoModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationId));

            CreateMap<ApplicationInfoModel, Application>()
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Organization, OrganizationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrganizationId));

            CreateMap<OrganizationModel, Organization>()
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Role, RoleModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<RoleModel, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
