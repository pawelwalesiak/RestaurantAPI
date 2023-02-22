using System.Diagnostics;
using Azure.Core;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware :IMiddleware
    {
        private Stopwatch _stopWatch;
        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            var elapsedMiliseconds = _stopWatch.ElapsedMilliseconds;
            if (elapsedMiliseconds / 1000 >4)
            {
                var message = $"Request {context.Request.Method} at {context.Request.Path} took{elapsedMiliseconds}";
                _logger.LogInformation(message);
            }
        }
    }
}
