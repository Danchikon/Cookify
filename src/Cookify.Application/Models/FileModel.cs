namespace Cookify.Application.Models;

public class FileModel : IDisposable, IAsyncDisposable
{
    public Stream Stream { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }

    public FileModel(Stream stream, string contentType, string name)
    {
        Stream = stream;
        Name = name;
        ContentType = contentType;
    }

    public void Dispose()
    {
        Stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
}