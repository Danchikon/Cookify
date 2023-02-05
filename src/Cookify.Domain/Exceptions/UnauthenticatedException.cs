using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions;

public class UnauthenticatedException : BusinessExceptionBase
{
    private UnauthenticatedException(string message) : base(message)
    {
    }
    
    public static UnauthenticatedException Create()
    {
        return new UnauthenticatedException($"User not authenticated");
    }
}