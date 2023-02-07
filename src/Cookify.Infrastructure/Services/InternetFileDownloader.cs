using System.Net;
using Cookify.Application.Services;
using Microsoft.Extensions.Logging;

namespace Cookify.Infrastructure.Services;

public class InternetFileDownloaderService : IInternetFileDownloaderService
{
    private static readonly SemaphoreSlim SemaphoreSlim = new(1);
    
    private readonly WebClient _webClient;
    private readonly ILogger<InternetFileDownloaderService> _logger;

    public InternetFileDownloaderService(WebClient webClient, ILogger<InternetFileDownloaderService> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }
    
    public async Task<Stream> DownloadAsync(Uri path, CancellationToken cancellationToken)
    {
        try
        {
            await SemaphoreSlim.WaitAsync(cancellationToken);

            _logger.LogInformation("Downloading {Url}", path.ToString());
            
            var fileBytes = await _webClient.DownloadDataTaskAsync(path);
            
            _logger.LogInformation("Downloaded {Url}", path.ToString());
            
            return new MemoryStream(fileBytes);
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }
}