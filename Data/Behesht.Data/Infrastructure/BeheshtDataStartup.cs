using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Behesht.Core.Data;
using Behesht.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Data.Infrastructure
{
    public static class BeheshtDataStartup
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(ISpRepository<>), typeof(SpRepository<>));

            return services;
        }

        public static IServiceCollection AddEfDbContext<TContext>(this IServiceCollection services, string connectionString, string migrationAssemblyName = "", params SaveChangesInterceptor[] SaveChangesInterceptors) where TContext : BeheshtDbContext
        {
            services.AddDbContextPool<IDbContext, TContext>(options =>
            {
                var optionBuilder = options.UseSqlServer(connectionString,
                    x =>
                    {
                        if (!string.IsNullOrEmpty(migrationAssemblyName))
                            x.MigrationsAssembly(migrationAssemblyName);
                    });
                if (SaveChangesInterceptors != null && SaveChangesInterceptors.Any())
                {
                    optionBuilder.AddInterceptors(SaveChangesInterceptors);
                }

            });
            return services;
        }

        public static IServiceCollection AddFullIdentityDbContext<TContext, TUser, TRole>(this IServiceCollection services,
            string connectionString,
            string migrationAssemblyName = "",
            params SaveChangesInterceptor[] SaveChangesInterceptors)

            where TContext : BeheshtIdentityDbContext<TUser, TRole, long>
            where TUser : IdentityUser<long>
            where TRole : IdentityRole<long>
        {
            services.AddDbContext<TContext>(options =>
             {
                 var optionBuilder = options.UseSqlServer(connectionString,
                        x =>
                        {
                            if (!string.IsNullOrEmpty(migrationAssemblyName))
                                x.MigrationsAssembly(migrationAssemblyName);
                        });
                 if (SaveChangesInterceptors != null && SaveChangesInterceptors.Any())
                 {
                     optionBuilder.AddInterceptors(SaveChangesInterceptors);
                 }
             }, ServiceLifetime.Scoped);
            services.AddScoped<IDbContext>(sp => sp.GetRequiredService<TContext>());
            services.AddIdentity<TUser, TRole>().AddEntityFrameworkStores<TContext>().AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection AddIdentityDbContext<TContext, TUser, TRole>(this IServiceCollection services, string connectionString)
            where TContext : IdentityDbContext<TUser, TRole, long>
            where TUser : IdentityUser<long>
            where TRole : IdentityRole<long>
        {
            services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<TUser, TRole>().AddEntityFrameworkStores<TContext>().AddDefaultTokenProviders();
            return services;
        }


    }
}
