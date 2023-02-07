namespace Cookify.Application.Services;

public interface IImageSearcherService
{
    Task<string?> FirstImageAsync(string query, CancellationToken cancellationToken);
}