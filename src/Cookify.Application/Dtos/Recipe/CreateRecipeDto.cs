namespace Cookify.Application.Dtos.Recipe;

public record CreateRecipeDto
{
    public string UkrainianTitle { get; init; } = null!;
    public string UkrainianInstruction { get; init; } = null!;
    public Guid CategoryId { get; init; }
    public bool IsPublic { get; init; }
}