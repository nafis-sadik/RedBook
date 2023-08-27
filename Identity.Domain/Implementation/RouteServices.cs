using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Domain;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Security.Claims;

namespace Identity.Domain.Implementation
{
    public class RouteServices : ServiceBase, IRouteServices
    {
        private IRepositoryBase<Route> _routeRepo;
        private IRepositoryBase<Role> _roleRepo;
        public RouteServices(
            ILogger<RouteServices> logger,
            IObjectMapper mapper,
            IUnitOfWorkManager unitOfWork,
            IClaimsPrincipalAccessor claimsPrincipalAccessor
        ) : base(logger, mapper, claimsPrincipalAccessor, unitOfWork)
        { }

        public async Task<RouteModel> AddRoute(RouteModel routeModel)
        {
            Route routeEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                routeEntity = Mapper.Map<Route>(routeModel);
                routeEntity = await _routeRepo.InsertAsync(routeEntity);
                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<RouteModel>(routeEntity);
        }

        public async Task DeleteRoute(int routeId)
        {
            using(var transaction = UnitOfWorkManager.Begin()) {
                _routeRepo = transaction.GetRepository<Route>();
                Route routeEntity = await _routeRepo.GetByIdAsync(routeId);
                await _routeRepo.DeleteAsync(routeEntity);
                await transaction.SaveChangesAsync();
            }
        }

        public async Task<RouteModel> GetRoute(int routeId)
        {
            Route routeEntity = await _routeRepo.GetByIdAsync(routeId);
            return Mapper.Map<RouteModel>(routeEntity);
        }

        public async Task<RouteModel> UpdateRoute(RouteModel routeModel)
        {
            Route routeEntity;
            using (var transaction = UnitOfWorkManager.Begin())
            {
                routeEntity = await _routeRepo.GetByIdAsync(routeModel.Id);
                routeEntity = Mapper.Map(routeModel, routeEntity);
                routeEntity = _routeRepo.Update(routeEntity);
                await transaction.SaveChangesAsync();
            }

            return Mapper.Map<RouteModel>(routeEntity);
        }
    }
}
