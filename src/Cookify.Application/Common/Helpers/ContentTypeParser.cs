namespace Cookify.Application.Common.Helpers;

public static class ContentTypeParser
{
    public static string GetImageContentTypeFromLink(string link)
    {
        return $"image/{link[(link.LastIndexOf('.') + 1)..]}";
    }
}