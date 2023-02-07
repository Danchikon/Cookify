using System.Text.Json.Serialization;
using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Recipe;

public record CreateRecipeCommand : CommandBase<Guid>, IAsyncDisposable
{
    public string UkrainianTitle { get; init; } = null!;
    public string UkrainianInstruction { get; init; } = null!;
    public Guid CategoryId { get; init; }
    public bool IsPublic { get; init; }
    public Stream ImageStream { get; init; } = null!;
    public string ImageContentType { get; init; } = null!;

    public async ValueTask DisposeAsync()
    { 
        await ImageStream.DisposeAsync();
    }
}