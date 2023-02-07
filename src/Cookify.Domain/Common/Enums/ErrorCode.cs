namespace Cookify.Domain.Common.Enums;

public enum ErrorCode
{
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    InternalServerError = 500,
    
    AlreadyExists = 600,

    #region Users

    PasswordsDoNotMatch = 1000,
    UserNotFound = 1001,
    Unauthenticated = 1002,
    UserAlreadyExists = 1003,

    #endregion
}