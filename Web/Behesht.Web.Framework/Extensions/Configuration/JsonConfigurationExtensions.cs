using Microsoft.Extensions.Configuration;
using System.IO;

namespace Behesht.Web.Framework.Extensions.Configuration
{
    public static class JsonConfigurationExtensions
    {
        public static IConfigurationBuilder AddBeheshtJsonFiles(this IConfigurationBuilder builder, string relativePath = "Configurations")
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            var files = Directory.GetFiles(path, "*.json");
            foreach (var jsonConfigFile in files)
            {
                builder.AddJsonFile(jsonConfigFile);
            }
            return builder;
        }
    }
}
