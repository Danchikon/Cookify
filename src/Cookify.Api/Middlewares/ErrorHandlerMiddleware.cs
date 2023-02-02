using Cookify.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Cookify.Api.Middlewares;

public sealed class ErrorHandlerMiddleware
{
    private readonly IActionResultExecutor<ObjectResult> _actionResultExecutor;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        IActionResultExecutor<ObjectResult> actionResultExecutor
    )
    {
        _next = next;
        _logger = logger;
        _actionResultExecutor = actionResultExecutor;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError("{ExceptionMessage}", exception.Message);
        var result = ExceptionToObjectResultConverter.Convert(exception);

        await SendResponseAsync(context, result);
    }

    private async Task SendResponseAsync(HttpContext context, ObjectResult objectResult)
    {
       await _actionResultExecutor.ExecuteAsync(new ActionContext { HttpContext = context }, objectResult);
    }
}