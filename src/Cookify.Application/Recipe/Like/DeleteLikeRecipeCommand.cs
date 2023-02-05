using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Recipe.Like;

public record DeleteLikeRecipeCommand(Guid RecipeId) : CommandBase;