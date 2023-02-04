using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions;

public class UnauthorizedException : BusinessExceptionBase
{
    private UnauthorizedException(string message) : base(message)
    {
    }
    
    public static UnauthorizedException Create()
    {
        return new UnauthorizedException("Unauthorized");
    }
}