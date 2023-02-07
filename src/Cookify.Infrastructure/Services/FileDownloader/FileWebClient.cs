using System.Net;
using Cookify.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Services.FileDownloader;

public class FileWebClient : WebClient
{
    private readonly InternetFileDownloaderOptions _options;
    public FileWebClient(IOptions<InternetFileDownloaderOptions> options)
    {
        _options = options.Value;
    }
    
    protected override WebRequest GetWebRequest(Uri uri)
    {
        var request = base.GetWebRequest(uri);
        request.Timeout = _options.TimeoutInSeconds * 1000;
        return request;
    }
}