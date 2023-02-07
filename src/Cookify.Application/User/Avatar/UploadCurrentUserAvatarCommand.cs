using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.User.Avatar;

public record UploadCurrentUserAvatarCommand(Stream FileStream, string ContentType) : CommandBase<string>, IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await FileStream.DisposeAsync();
    }
}