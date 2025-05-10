namespace Identity.WebAPI.Configurations
{
    public static class CacheConfig
    {
        public static readonly TimeSpan DefaultExpireTime = TimeSpan.FromMinutes(30);

        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            //// Add support for distributed caching
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration["Cache:Redis:ConnectionString"];
            //    options.InstanceName = configuration["Cache:Redis:InstanceName"];
            //});

            //// Memory caching
            //services.AddSingleton<ICacheSerializer, DefaultCacheSerializer>();
            //services.AddSingleton<ICacheManager, DistributedCacheManager>();
            //services.AddSingleton<ICachingConfiguration, CachingConfiguration>(ctx =>
            //{
            //    var config = new CachingConfiguration();
            //    config.ConfigureAll(cache =>
            //    {
            //        cache.DefaultAbsoluteExpireTime = DefaultExpireTime;
            //    });
            //    return config;
            //});

            //////Distributed Lock
            //services.AddSingleton<IDistributedLockManager, RedisLockManager>(ctx =>
            //{
            //    var connectionString = configuration["Cache:Redis:ConnectionString"];
            //    var instanceName = configuration["Cache:Redis:InstanceName"];
            //    var config = new RedisLockManager(connectionString, instanceName);

            //    return config;
            //});
        }
    }
}
