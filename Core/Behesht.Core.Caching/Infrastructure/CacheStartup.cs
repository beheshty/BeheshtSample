using Microsoft.Extensions.DependencyInjection;
using Behesht.Core.Caching;
using Behesht.Core.Caching.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class CacheStartup
    {
        /// <summary>
        /// Add Redis Distributed Cache
        /// </summary>
        /// <param name="services"></param>
        /// <param name="endpoint">redis endpoint</param>
        /// <param name="port">redis port</param>
        /// <param name="caheKeyPrefix">if empty or null, the calling assembly name will be used instead</param>
        /// <returns></returns>
        public static IServiceCollection AddBeheshtRedisDistributedCache(this IServiceCollection services, string endpoint, int port, string caheKeyPrefix = null)
        {
            if (string.IsNullOrEmpty(caheKeyPrefix))
            {
                caheKeyPrefix = Assembly.GetCallingAssembly().GetName().Name;
            }

            services.AddSingleton(new DistributedCacheConfigs()
            {
                KeyPrefix = caheKeyPrefix,
                Endpoint = endpoint,
                Port = port
            });

            services.AddSingleton(new CacheEnableConfig()
            {
                DistributedCacheEnabled = true
            });

            var configuration = $"{endpoint}:{port},abortConnect=false";

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration));

            services.AddStackExchangeRedisCache(p =>
            {
                p.Configuration = configuration;
                    //p.ConfigurationOptions.EndPoints.Add(configuration);
            });

            services.AddScoped<IBeheshtDistributedCacheManager, BeheshtDistributedCacheManager>();
            return services;
        }

        public static IServiceCollection AddBeheshtMemoryCache(this IServiceCollection services)
        {
            services.AddSingleton(new CacheEnableConfig()
            {
                DistributedCacheEnabled = false
            });
            services.AddSingleton<IBeheshtCacheManager, BeheshtMemoryCacheManager>();
            return services;
        }
    }
}
