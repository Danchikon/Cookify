namespace Cookify.Domain.Common.Exceptions;

public abstract class BusinessExceptionBase : Exception
{
    internal BusinessExceptionBase(string message): base(message) {}
}