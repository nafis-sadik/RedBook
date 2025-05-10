﻿using AutoMapper;
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

            // Putting a create date is gonna cause problem on update operations
            // It wouldn't help anything except create
            CreateMap<OrganizationModel, Organization>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.OrganizationAddress))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.BusinessLogo))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatededBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Role, RoleModel>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<RoleModel, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<RouteModel, Route>()
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId))
                .ForMember(dest => dest.RoutePath, opt => opt.MapFrom(src => src.RouteValue))
                .ForMember(dest => dest.ParentRouteId, opt => opt.MapFrom(src => src.ParentRouteId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description == null ? "" : src.Description));

            CreateMap<Route, RouteModel>()
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId))
                .ForMember(dest => dest.ApplicationName, opt => opt.Ignore())
                .ForMember(dest => dest.RouteValue, opt => opt.MapFrom(src => src.RoutePath))
                .ForMember(dest => dest.ParentRouteId, opt => opt.MapFrom(src => src.ParentRouteId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description == null ? "" : src.Description));
        }
    }
}
