using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RestApi.Middlewares
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomHeaderMiddleware> _logger;

        public CustomHeaderMiddleware(RequestDelegate next, ILogger<CustomHeaderMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check for a custom request header
            if (context.Request.Headers.TryGetValue("X-MY-APP-WORK", out var headerValue))
            {
                _logger.LogInformation($"Request Header: X-MY-APP-WORK = {headerValue}");
            }
            else
            {
                _logger.LogWarning("X-MY-APP-WORK not found");
            }

            // Add a custom response header
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("X-I-AM-THE-BEST", "HereIsProof");
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
