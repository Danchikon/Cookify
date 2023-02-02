namespace Cookify.Application.Services;

public interface ICurrentUserService
{
    Guid GetUserId();
}