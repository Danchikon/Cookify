using Cookify.Domain.Common.Enums;
using Cookify.Domain.Common.Exceptions;

namespace Cookify.Domain.Exceptions.Users;

public class UserAlreadyExistsException : AlreadyExistsException
{
    private UserAlreadyExistsException(string message) : base(message, ErrorCode.UserAlreadyExists)
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