namespace Inventory.WebAPI.Configurations
{
    public static class CorsConfig
    {
        public const string Policy = "AllowSpecificOrigin";

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(opts =>
             opts.AddPolicy(Policy, builder =>
             {
                 var corsConfiguration = configuration.GetSection("CORS:AllowedOrigins");

                 var corsOrigins = corsConfiguration.Get<string[]>();
                 if (corsOrigins != null)
                 {
                     builder
                         .WithOrigins(corsOrigins)
                         .WithExposedHeaders("x-total-count")
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials();
                 }
             }));

            return services;
        }
    }
}
