using Inventory.Data.Entities;
using Inventory.Domain.Abstraction;
using Inventory.Domain.Abstraction.CRM;
using Inventory.Domain.Abstraction.Product;
using Inventory.Domain.Abstraction.Purchase;
using Inventory.Domain.Implementation;
using Inventory.Domain.Implementation.CRM;
using Inventory.Domain.Implementation.Product;
using Inventory.Domain.Implementation.Purchase;
using RedBook.Core;
using RedBook.Core.Security;

namespace Inventory.WebAPI.Configurations
{
    public static class DependencyResolver
    {
        public static void RosolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // DB Context & Other relevant mappings for Blume Core Library
            CoreDependencyResolver<RedbookInventoryContext>.RosolveCoreDependencies(services, configuration);

            // Services
            services.AddScoped<IClaimsPrincipalAccessor, HttpContextClaimsPrincipalAccessor>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddScoped<ICommonAttributeService, CommonAttributeService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IInvoiceDetailsService, InvoiceDetailsService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<ICustomerServices, CustomerServices>();
        }
    }
}
