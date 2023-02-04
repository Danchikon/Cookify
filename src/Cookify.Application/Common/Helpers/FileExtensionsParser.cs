namespace Cookify.Application.Common.Helpers;

public static class FileExtensionsParser
{
    public static string ParseFromLink(string link)
    {
        return link[(link.LastIndexOf('.') + 1)..];
    }
}