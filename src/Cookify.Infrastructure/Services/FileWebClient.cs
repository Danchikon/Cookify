using System.Net;

namespace Cookify.Infrastructure.Services;

public class FileWebClient : WebClient
{
    protected override WebRequest GetWebRequest(Uri uri)
    {
        var request = base.GetWebRequest(uri);
        request.Timeout = 10 * 60 * 1000;
        return request;
    }
}