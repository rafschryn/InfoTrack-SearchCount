using Microsoft.AspNetCore.Diagnostics;
using SearchCount.Shared;
using System.Diagnostics;

namespace SearchCount.API
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            _logger.LogError(exception, "Could not process the request on {MachineName}. TraceId: {TraceId}",
                Environment.MachineName, traceId);

            var (statusCode, title) = MapException(exception);

            await Results.Problem(
                title: title,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?>
                    {
                        {"tradeId", traceId}
                    }
                ).ExecuteAsync(httpContext);

            return true;
        }

        private static (int statusCode, string title) MapException(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => (StatusCodes.Status400BadRequest, exception.Message),
                CaseNotHandledException => (StatusCodes.Status400BadRequest, exception.Message),
                SearchEngineException => (StatusCodes.Status400BadRequest, exception.Message),
                RequestValidationException => (StatusCodes.Status400BadRequest, exception.Message),
                InvalidCastException => (StatusCodes.Status400BadRequest, exception.Message),

                _ => (StatusCodes.Status500InternalServerError, "Oops something went wrong!")
            };
        }
    }
}
