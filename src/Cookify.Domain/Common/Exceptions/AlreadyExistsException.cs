using Cookify.Domain.Common.Enums;

namespace Cookify.Domain.Common.Exceptions;

public class AlreadyExistsException : BusinessExceptionBase
{
    protected AlreadyExistsException(string message, ErrorCode? code = null) 
        : base(message, code ?? ErrorCode.AlreadyExists) {}
}