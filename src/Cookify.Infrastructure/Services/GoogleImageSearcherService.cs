using AngleSharp;
using Cookify.Application.Services;
using Cookify.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Services;

public class GoogleImageSearcherService : IImageSearcherService
{
    private static readonly object LockObject = new();
    
    private static Lazy<SemaphoreSlim> _semaphoreSlim = new();

    private readonly GoogleImageSearcherOptions _options;
    private readonly IBrowsingContext _browsingContext;
    private readonly ILogger<GoogleImageSearcherService> _logger;

    public GoogleImageSearcherService(
        IBrowsingContext browsingContext, 
        IOptions<GoogleImageSearcherOptions> options,
        ILogger<GoogleImageSearcherService> logger
        )
    {
        _browsingContext = browsingContext;
        _logger = logger;
        _options = options.Value;

        lock (LockObject)
        {
            if (_semaphoreSlim.IsValueCreated)
            {
                return;
            }
            
            _semaphoreSlim = new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(_options.MaximumConcurrency));
            var _ = _semaphoreSlim.Value;
        }
    }
    
    public async Task<string?> FirstImageAsync(string query, CancellationToken cancellationToken)
    {
        try
        {
            await _semaphoreSlim.Value.WaitAsync(cancellationToken);
            
            _logger.LogInformation("Searching image {Query}", query);
            
            var document = await _browsingContext.OpenAsync($"{_options.Url}/search?q={query.Replace(' ', '+')}&tbm=isch", cancellationToken);

            var imageLink = document.Images.FirstOrDefault(imageElement => imageElement.ClassName == "yWs4tf")?.Source;

            _logger.LogInformation("Image searched {Link}", imageLink);
        
            return imageLink;
        }
        finally
        {
            _semaphoreSlim.Value.Release();
        }
    }
}