namespace Cookify.Application.Services;

public interface IInternetFileDownloaderService
{
    Task<Stream> DownloadAsync(Uri path);
}