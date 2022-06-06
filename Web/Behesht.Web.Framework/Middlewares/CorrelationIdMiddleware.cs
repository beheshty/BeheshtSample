using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Behesht.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdOptions _options;

        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));

            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(_options.Header, out StringValues correlationId))
            {
                context.TraceIdentifier = correlationId;
            }

            if (_options.IncludeInResponse)
            {
                // apply the correlation ID to the response header for client side tracking
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(_options.Header))
                    {
                        context.Response.Headers.Add(_options.Header, new[] { context.TraceIdentifier });
                    }
                    return Task.CompletedTask;
                });
            }

            return _next(context);
        }
    }
}
