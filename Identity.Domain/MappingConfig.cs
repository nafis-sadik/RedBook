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

            CreateMap<Organization, OrganizationModel>();

            // Putting a create date is gonna cause problem on update operations
            // It wouldn't help anything except create
            CreateMap<OrganizationModel, Organization>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.UpdatededBy, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Role, RoleModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<RoleModel, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
