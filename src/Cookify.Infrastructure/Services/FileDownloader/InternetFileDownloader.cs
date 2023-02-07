using System.Net;
using Cookify.Application.Services;
using Cookify.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Services.FileDownloader;

public class InternetFileDownloaderService : IInternetFileDownloaderService
{
    private static readonly object LockObject = new();
    
    private static Lazy<SemaphoreSlim> _semaphoreSlim = new();
    
    private readonly WebClient _webClient;
    private readonly ILogger<InternetFileDownloaderService> _logger;

    public InternetFileDownloaderService(
        IOptions<InternetFileDownloaderOptions> options,
        WebClient webClient, 
        ILogger<InternetFileDownloaderService> logger
        )
    {
        _webClient = webClient;
        _logger = logger;
        
        lock (LockObject)
        {
            if (_semaphoreSlim.IsValueCreated)
            {
                return;
            }
            
            _semaphoreSlim = new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(options.Value.MaximumConcurrency));
            var _ = _semaphoreSlim.Value;
        }
    }
    
    public async Task<Stream> DownloadAsync(Uri path, CancellationToken cancellationToken)
    {
        try
        {
            await _semaphoreSlim.Value.WaitAsync(cancellationToken);

            _logger.LogInformation("Downloading {Url}", path.ToString());
            
            var fileBytes = await _webClient.DownloadDataTaskAsync(path);
            
            _logger.LogInformation("Downloaded {Url}", path.ToString());
            
            return new MemoryStream(fileBytes);
        }
        finally
        {
            _semaphoreSlim.Value.Release();
        }
    }
}