using Cookify.Application.Dtos.Authentication;

namespace Cookify.Application.Services;

public interface ICurrentUserService
{
    Guid GetUserId();
}