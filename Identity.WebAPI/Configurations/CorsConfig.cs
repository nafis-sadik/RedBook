using Microsoft.Extensions.DependencyInjection;

namespace Identity.WebAPI.Configurations
{
    /// <summary>
    /// Extension methods for setting up CORS Configurations.
    /// </summary>
    public static class CorsConfig
    {
        /// <summary>
        /// CORS Policy name of custom configured policy
        /// </summary>
        public static string CorsPolicy = "CustomCors";

        /// <summary>
        /// Registers CORS allowed domains from appsettings.json under "CORS:AllowedOrigins"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public static void AddCorsIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(opts =>
            opts.AddPolicy(CorsPolicy,
                 policy =>
                 {
                     var corsConfiguration = configuration.GetSection("CORS:AllowedHosts");

                     var corsOrigins = corsConfiguration.Get<string[]>();
                     if (corsOrigins != null)
                     {
                         policy
                             .WithOrigins(corsOrigins)
                             .WithExposedHeaders("x-total-count")
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials();
                     }
                 }));
        }
    }
}
