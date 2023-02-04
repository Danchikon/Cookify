using AngleSharp;
using AngleSharp.Html.Dom;
using Cookify.Application.Services;
using Microsoft.Extensions.Logging;

namespace Cookify.Infrastructure.Services;

public class GoogleImageSearcherService : IImageSearcherService
{
    private const string GoogleBaseUrl = "https://www.google.com.ua";
    
    private static readonly SemaphoreSlim SemaphoreSlim = new(5);

    private readonly IBrowsingContext _browsingContext;
    private readonly ILogger<GoogleImageSearcherService> _logger;

    public GoogleImageSearcherService(IBrowsingContext browsingContext, ILogger<GoogleImageSearcherService> logger)
    {
        _browsingContext = browsingContext;
        _logger = logger;
    }
    
    public async Task<string?> FirstImageAsync(string query)
    {
        try
        {
            await SemaphoreSlim.WaitAsync();
            
            _logger.LogInformation("Searching image {Query}", query);
            
            var document = await _browsingContext.OpenAsync($"{GoogleBaseUrl}/search?q={query.Replace(' ', '+')}&tbm=isch");

            var imageLink = document.Images.FirstOrDefault(imageElement => imageElement.ClassName == "yWs4tf")?.Source;

            _logger.LogInformation("Image searched {Link}", imageLink);
        
            return imageLink;
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }
}