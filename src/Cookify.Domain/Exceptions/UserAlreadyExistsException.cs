using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions;

public class UserAlreadyExistsException : BusinessExceptionBase
{
    private UserAlreadyExistsException(string message) : base(message)
    {
    }

    public static UserAlreadyExistsException CreateForEmail(string email)
    {
        return new UserAlreadyExistsException($"User with email {email} already exists");
    }
    
    public static UserAlreadyExistsException CreateForUsername(string username)
    {
        return new UserAlreadyExistsException($"User with username {username} already exists");
    }
}