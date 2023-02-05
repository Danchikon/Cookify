using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.User.Avatar;

public record UploadCurrentUserAvatarCommand(Stream FileStream, string ContentType) : CommandBase<string>, IDisposable, IAsyncDisposable
{
    public void Dispose()
    {
        FileStream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await FileStream.DisposeAsync();
    }
}