﻿using Identity.Data.Entities;
using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IRouteServices
    {
        Task<RouteModel> AddRoute(RouteModel routeModel);
        Task<RouteModel> UpdateRoute(RouteModel routeModel);
        Task DeleteRoute(int routeId);
        Task<RouteModel?> GetRoute(int routeId);
        Task<List<RouteType>> GetRouteTypes();
        Task<IEnumerable<RouteModel>> GetRoutesByRoleId(int roleId);
        Task<IEnumerable<RouteModel>> GetAppRoutes();
        Task<IEnumerable<RouteModel>> GetAppMenuRoutes();
        Task<PagedModel<RouteModel>> GetPagedRoutes(PagedModel<RouteModel> pagedRoutes);
    }
}
