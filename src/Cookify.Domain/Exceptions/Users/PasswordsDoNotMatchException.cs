using Cookify.Domain.Common.Enums;
using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions.Users;

public class PasswordsDoNotMatchException : BusinessExceptionBase
{
    private PasswordsDoNotMatchException(string message) : base(message, ErrorCode.PasswordsDoNotMatch)
    {
    }
    
    public static PasswordsDoNotMatchException Create()
    {
        return new PasswordsDoNotMatchException("Passwords do not match");
    }
}