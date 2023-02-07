using System.ComponentModel.DataAnnotations;
using System.Net;
using Cookify.Application.Common.Dtos;
using Cookify.Domain.Common.Enums;
using Cookify.Domain.Common.Exceptions;
using Cookify.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Cookify.Api.Common.Helpers;

public static class ExceptionToObjectResultConverter
{
    public static Func<Exception, HttpStatusCode>? CustomMap { get; set; }

    public static ObjectResult Convert(Exception exception)
    {
        var statusCode = exception switch
        {
            UnauthorizedException => HttpStatusCode.Unauthorized,
            UnauthenticatedException => HttpStatusCode.Unauthorized,
            NotImplementedException => HttpStatusCode.NotImplemented,
            InvalidOperationException => HttpStatusCode.Conflict,
            AlreadyExistsException => HttpStatusCode.Conflict,
            ArgumentException => HttpStatusCode.BadRequest,
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            _ => CustomMap?.Invoke(exception) ?? HttpStatusCode.InternalServerError
        };

        var errors = new List<string> { exception.Message };

        var innerException = exception.InnerException;
        while (innerException is not null)
        {
            errors.Add(innerException.Message);
            innerException = innerException.InnerException;
        }

        var errorCode = (int)statusCode;

        if (exception is BusinessExceptionBase businessExceptionBase)
        {
            errorCode = (int)businessExceptionBase.Code;
        }

        var error = new ErrorDto
        {
            Title = exception.GetType().Name,
            Messages = errors,
            Code = errorCode
        };

        return new ObjectResult(error) { StatusCode = (int)statusCode };
    }
}