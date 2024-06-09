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
            if (context.Request.Headers.TryGetValue("X-JUST-SOME-UNNECESSARY-HEADER", out var headerValue))
            {
                _logger.LogInformation($"Request Header: X-JUST-SOME-UNNECESSARY-HEADER = {headerValue}");
            }
            else
            {
                _logger.LogWarning("X-JUST-SOME-UNNECESSARY-HEADER not found");
            }

            // Add a custom response header
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("X-ANOTHER-ONE", "StillDoingNothing");
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
