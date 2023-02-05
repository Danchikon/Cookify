using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Recipe.Like;

public record CreateLikeRecipeCommand(Guid RecipeId) : CommandBase;