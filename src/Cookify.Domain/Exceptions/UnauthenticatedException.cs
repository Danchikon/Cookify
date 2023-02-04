using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions;

public class UnauthenticatedException : BusinessExceptionBase
{
    private UnauthenticatedException(string message) : base(message)
    {
    }
    
    public static UnauthenticatedException Create(string email)
    {
        return new UnauthenticatedException($"User {email} not authenticated");
    }
}