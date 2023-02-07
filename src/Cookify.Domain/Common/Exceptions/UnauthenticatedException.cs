using Cookify.Domain.Common.Enums;

namespace Cookify.Domain.Common.Exceptions;

public class UnauthenticatedException : BusinessExceptionBase
{
    private UnauthenticatedException(string message) : base(message, ErrorCode.Unauthenticated)
    {
    }
    
    public static UnauthenticatedException Create()
    {
        return new UnauthenticatedException($"User not authenticated");
    }
}