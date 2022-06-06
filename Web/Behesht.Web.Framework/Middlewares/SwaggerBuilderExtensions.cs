using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Middlewares
{
    public static class SwaggerBuilderExtensions
    {
        public static IApplicationBuilder UseBeheshtSwagger(this IApplicationBuilder app, string appName)
        {
            app.UseBeheshtSwagger(appName, null);
            return app;
        }

        public static IApplicationBuilder UseBeheshtSwagger(this IApplicationBuilder app, string appName, string clientId, string clientSecret = null)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", appName);
                if (!string.IsNullOrEmpty(clientId))
                {
                    c.OAuthClientId(clientId);
#if DEBUG
                    if (!string.IsNullOrEmpty(clientSecret))
                    {
                        c.OAuthClientSecret(clientSecret);
                    }
#endif
                }
                c.OAuthAppName(appName);
                c.OAuthUsePkce();
            });

            return app;
        }
    }
}
