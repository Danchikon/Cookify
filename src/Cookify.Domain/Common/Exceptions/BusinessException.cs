using Cookify.Domain.Common.Enums;

namespace Cookify.Domain.Common.Exceptions;

public abstract class BusinessExceptionBase : Exception
{
    public readonly ErrorCode Code;

    internal BusinessExceptionBase(string message, ErrorCode code) : base(message)
    {
        Code = code;
    }
}