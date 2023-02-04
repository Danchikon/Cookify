using System.ComponentModel.DataAnnotations;
using System.Net;
using Cookify.Application.Common.Dtos;
using Cookify.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Cookify.Api.Common.Helpers;

public static class ExceptionToObjectResultConverter
{
    public static Func<Exception, HttpStatusCode>? CustomMap { get; set; }

    public static ObjectResult Convert(Exception exception)
    {
        var code = exception switch
        {
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            NotImplementedException => HttpStatusCode.NotImplemented,
            InvalidOperationException => HttpStatusCode.Conflict,
            ArgumentException => HttpStatusCode.BadRequest,
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            _ => CustomMap?.Invoke(exception) ?? HttpStatusCode.InternalServerError
        };

        var errors = new List<string> { exception.Message };

        if (exception.InnerException is not null)
        {
            errors.Add(exception.InnerException.Message);
        }

        var error = new ErrorDto
        {
            Title = exception.GetType().Name,
            Messages = errors
        };

        return new ObjectResult(error) { StatusCode = (int)code };
    }
}