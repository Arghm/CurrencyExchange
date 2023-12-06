using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace CurrencyExchange.WebApi.Host.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? new NullLogger<ErrorHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                string result;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                result = JsonSerializer.Serialize(response.StatusCode == StatusCodes.Status400BadRequest
                    ? new
                    {
                        error = "Argument exception",
                        message = error?.Message
                    }
                    : new
                    {
                        error = "Request error",
                        message = error?.Message
                    });

                _logger.LogError(result);

                await response.WriteAsync(result);
            }
        }
    }
}
