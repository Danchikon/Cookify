using Cookify.Domain.Common.Enums;

namespace Cookify.Domain.Common.Exceptions;

public class NotFoundException : BusinessExceptionBase
{
    protected NotFoundException(string message, ErrorCode? code = null) 
        : base(message, code ?? ErrorCode.NotFound) {}

    public static NotFoundException Create<T>(ErrorCode? code = null)
    {
        return new($"{typeof(T).Name} not found", code);
    }

    public static NotFoundException Create<T>(Guid id, ErrorCode? code = null)
    {
        return new($"{typeof(T).Name} with id {id} not found", code);
    }
}