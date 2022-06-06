using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Behesht.Web.Framework.Middlewares;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Behesht.Web.Framework.Infrastructure;
using Behesht.Web.Framework.Data;
using Behesht.Data;
using Behesht.Data.CatalogSample;
using Behesht.Data.Infrastructure;
using Behesht.Data.CatalogSample.Infrastructure;


namespace Behesht.Web.Api.CatalogSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddEfDbContext<CatalogSampleContext>(connectionString, SaveChangesInterceptors: new WebSaveChangesInterceptor());

            BaseStartup.ConfigureApplicationServices(services);

            services.AddCatalogEfRepositories();

            services.AddBeheshtSwagger();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseBeheshtSwagger("Behesht.Web.Api.CatalogSample");

            dbContext.Migrate();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
