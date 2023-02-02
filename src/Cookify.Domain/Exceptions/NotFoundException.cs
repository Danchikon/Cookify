using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions;

public class NotFoundException : BusinessExceptionBase
{
    private NotFoundException(string message): base(message) {}

    public static NotFoundException Create<T>()
    {
        return new($"{typeof(T).Name} not found");
    }

    public static NotFoundException Create<T>(Guid id)
    {
        return new($"{typeof(T).Name} with id {id} not found");
    }
}