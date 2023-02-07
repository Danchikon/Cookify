using Cookify.Domain.Common.Enums;

namespace Cookify.Domain.Common.Exceptions;

public class UnauthorizedException : BusinessExceptionBase
{
    private UnauthorizedException(string message) : base(message, ErrorCode.Unauthorized)
    {
    }
    
    public static UnauthorizedException Create()
    {
        return new UnauthorizedException("Unauthorized");
    }
}