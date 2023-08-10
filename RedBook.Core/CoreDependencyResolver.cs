﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RedBook.Core.AutoMapper;
using RedBook.Core.EntityFramework;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core
{
    public static class CoreDependencyResolver<TDbContext> where TDbContext : DbContext
    {
        public static void RosolveCoreDependencies(IServiceCollection services)
        {
            services.AddDbContext<DbContext, TDbContext>();

            // Unit Of Work
            services.AddScoped<IUnitOfWorkManager, EFUnitOfWorkManager>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            // Claims
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Object pooling
            services.AddObjectMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
