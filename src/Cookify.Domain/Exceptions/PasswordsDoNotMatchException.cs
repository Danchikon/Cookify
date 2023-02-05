using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions;

public class PasswordsDoNotMatchException : BusinessExceptionBase
{
    private PasswordsDoNotMatchException(string message) : base(message)
    {
    }
    
    public static PasswordsDoNotMatchException Create()
    {
        return new PasswordsDoNotMatchException("Passwords do not match");
    }
}