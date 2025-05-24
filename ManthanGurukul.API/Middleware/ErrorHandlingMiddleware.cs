using ManthanGurukul.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace ManthanGurukul.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            var errorResponse = new { errormessage = string.Empty };

            switch (ex)
            {
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    _logger.LogError($"Error Details: {ex}");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                errorResponse = new { errormessage = "Something went wrong please try after sometime." };
            }
            else
            {
                errorResponse = new { errormessage = ex.Message };
            }

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
