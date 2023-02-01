namespace Cookify.Domain.Common.Exceptions;

public class NotFoundException : BusinessExceptionBase
{
    public NotFoundException(string message): base(message) {}

    public static NotFoundException Create<T>()
    {
        return new($"{typeof(T).Name} not found");
    }

    public static NotFoundException Create<T>(Guid id)
    {
        return new($"{typeof(T).Name} with id {id} not found");
    }
}