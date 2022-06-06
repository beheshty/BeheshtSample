using Microsoft.Extensions.DependencyInjection;
using Behesht.Core.Caching;
using Behesht.Core.Infrastructure.IO;
using Behesht.Services.Events;
using Behesht.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Services.Infrustructure
{
    public static class BeheshtServiceStartup
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services)
        {
            return services.Scan(scan =>
            scan.FromAssemblyDependencies(Assembly.GetEntryAssembly()).AddClasses(cl => cl.AssignableTo(typeof(IBaseService<,>))).AsImplementedInterfaces().WithScopedLifetime());
        }

        public static IServiceCollection AddSecurityServices(this IServiceCollection services)
        {
            return services.AddScoped<IEncryptionService, EncryptionService>();
        }

        public static IServiceCollection AddEventPublisher(this IServiceCollection services)
        {
            return services.AddSingleton<IEventPublisher, EventPublisher>();
        }

        public static IServiceCollection AddFileServices(this IServiceCollection services)
        {
            return services.AddSingleton<IFileService, FileService>();
        }

        /// <summary>
        /// requires file and cache services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="localizationFilesPath"></param>
        /// <returns></returns>
        public static IServiceCollection AddBeheshtLocalizerServices(this IServiceCollection services, string localizationFilesPath = "Localization")
        {
            return services.AddSingleton<IBeheshtLocalizerService, BeheshtLocalizerService>(x =>
                new BeheshtLocalizerService(localizationFilesPath, x.GetRequiredService<IFileService>(), x.GetRequiredService<IBeheshtCacheManager>()));
        }
    }
}
