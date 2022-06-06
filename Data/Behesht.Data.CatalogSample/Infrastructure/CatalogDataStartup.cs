using Behesht.Data.CatalogSample.Repositories.Catalog;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Data.CatalogSample.Infrastructure
{
    public static class CatalogDataStartup
    {
        public static IServiceCollection AddCatalogEfRepositories(this IServiceCollection services)
        {
            //Repositories will add as needed, otherwise EfRepository which implemented as IRepository will do the job
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
