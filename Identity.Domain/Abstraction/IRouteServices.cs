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
        Task<IEnumerable<RouteModel>> GetAllRoutes(string userId);
        Task<PagedModel<RouteModel>> GetPagedRoutes(PagedModel<RouteModel> pagedRoutes);
    }
}
