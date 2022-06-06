using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Behesht.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Middlewares
{
    public static class ErrorHandlerBuilderExtensions
    {
        public static IApplicationBuilder UseBeheshtErrorHandlerForApi(this IApplicationBuilder app, bool includeDevelopmentDetails = false)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
                BaseApiResult errorResult = new()
                {
                    Data = includeDevelopmentDetails ? exceptionHandler.Error.ToString() : "An Error Occurred!", 
                    Status = 500,
                    Success = false,
                    Message = exceptionHandler.Error.Message
                };
                await context.Response.WriteAsJsonAsync(errorResult);
            }));
        }
    }
}
