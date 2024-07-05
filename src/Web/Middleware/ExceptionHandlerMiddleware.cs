using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Web.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILoggerFactory _logger;
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(IWebHostEnvironment env,ILoggerFactory logger,RequestDelegate next)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var result = HandleServerError(context, ex, options);
                result = HandleResult(context, ex, result, options);
                await context.Response.WriteAsync(result);
            }
        }

        private static string HandleServerError(HttpContext context, Exception ex, JsonSerializerOptions options)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new ApiToReturn(500, ex.Message), options);
            return result;
        }

        private string HandleResult(HttpContext context, Exception ex, string result, JsonSerializerOptions options)
        {
            switch (ex)
            {
                case NotFoundEntityException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new ApiToReturn(404, notFoundException.Message, 
                        notFoundException.Messages, ex.Message));
                    break;

                case BadRequestEntityException badRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new ApiToReturn(400, badRequestException.Message, 
                        badRequestException.Messages, ex.Message));
                    break;

                case ValidationEntityException validationEntityException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new ApiToReturn(400, validationEntityException.Message, 
                        validationEntityException.Messages, ex.Message));
                    break;
            }
            return result;
        }
    }
}
