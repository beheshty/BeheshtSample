using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Behesht.Data.Infrastructure;
using Behesht.Services.Infrustructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Behesht.Data;
using Behesht.Core.Infrastructure.Types;
using Behesht.Services;
using System.Reflection;
using Behesht.Web.Framework.ActionFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Behesht.Web.Framework.Reflections;
using System.Text.Json.Serialization;
using Behesht.Core;
using Behesht.Core.Infrastructure.IO;
using Behesht.Web.Framework.Authorization;
using Behesht.Web.Framework.Enums;
using Mapster;
using MapsterMapper;

namespace Behesht.Web.Framework.Infrastructure
{
    public static class BaseStartup
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,
            IEnumerable<Type> actionFilters = null,
            IEnumerable<JsonConverter> jsonConverters = null)
        {
            CoreCommonHelper.DefaultFileService = new FileService();

            services.AddSingleton<ITypeFinder, TypeFinder>();
            services.AddSingleton<IWebTypeFinder, WebTypeFinder>();

            services.AddFileServices();
            services.AddBeheshtMemoryCache();
            services.AddBeheshtLocalizerServices();


            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelActionFilterAttribute));
                if (actionFilters != null && actionFilters.Any())
                {
                    foreach (var item in actionFilters)
                    {
                        options.Filters.Add(item);
                    }
                }
            }).AddJsonOptions(p =>
            {
                if (jsonConverters != null && jsonConverters.Any())
                {
                    foreach (var item in jsonConverters)
                    {
                        p.JsonSerializerOptions.Converters.Add(item);
                    }
                }
            });


            services.AddHttpContextAccessor();


            services.AddEfRepositories();

            services.AddMapper();

            services.AddBaseServices();

            StoreServiceProvider(services);
            return services;
        }

        public static IServiceCollection AddBeheshtSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                ConfigSwagger(c);
            });
            return services;
        }

        private static void ConfigSwagger(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            //var filePath = Path.Combine(AppContext.BaseDirectory, "API.xml");
            //c.IncludeXmlComments(filePath);
        }

        public static IServiceCollection AddBeheshtSwagger(this IServiceCollection services, SwaggerAuthorityFlows authorityFlows, string idsBaseAddress, string swaggerUiScope)
        {
            services.AddSwaggerGen(c =>
            {
                ConfigSwagger(c);
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = GetFlow(authorityFlows, idsBaseAddress, swaggerUiScope)
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>(swaggerUiScope);
            });
            return services;
        }

        private static OpenApiOAuthFlows GetFlow(SwaggerAuthorityFlows authorityFlows, string idsBaseAddress, string swaggerUiScope)
        {
            return authorityFlows switch
            {
                SwaggerAuthorityFlows.ClientCredentials => new OpenApiOAuthFlows()
                {
                    ClientCredentials = GetOpenApiOAuthFlow(idsBaseAddress, swaggerUiScope)
                },
                SwaggerAuthorityFlows.AuthorizationCode => new OpenApiOAuthFlows()
                {
                    AuthorizationCode = GetOpenApiOAuthFlow(idsBaseAddress, swaggerUiScope)
                },
                _ => null
            };
        }

        private static OpenApiOAuthFlow GetOpenApiOAuthFlow(string idsBaseAddress, string swaggerUiScope)
        {
            return new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{idsBaseAddress}/connect/authorize"),
                TokenUrl = new Uri($"{idsBaseAddress}/connect/token"),
                Scopes = new Dictionary<string, string>
                            {
                                {swaggerUiScope, $"SwaggerUI Client for {Assembly.GetEntryAssembly().GetName().Name}"}
                            }
            };
        }

        private static void StoreServiceProvider(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            ServicesCommonHelper.DefaultLocalizer = provider.GetRequiredService<IBeheshtLocalizerService>();
            ServiceProviderStore.StoreProvider(provider);
        }

        private static IServiceCollection AddMapper(this IServiceCollection services)
        {
            //var typeFinder = new TypeFinder();
            //services.AddAutoMapper(typeFinder.FindClassesOfType<Profile>().ToArray());
            var config =  TypeAdapterConfig.GlobalSettings;
            var typeFinder = new TypeFinder();
            var registerTypes = typeFinder.FindClassesOfType<IRegister>();
            IEnumerable<Lazy<IRegister>> lazyList = registerTypes.Select(p => new Lazy<IRegister>(() => (IRegister)Activator.CreateInstance(p))).ToList();
            config.Apply(lazyList);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IEnumerable<string> audience, IEnumerable<string> issuer, string secret, Action<AuthorizationOptions> configureAction)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudiences = audience,
                        ValidIssuers = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });
            services.AddAuthorization(configureAction);
            return services;
        }
    }
}
