using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Recipe.Favorite;

public record DeleteFavoriteRecipeCommand(Guid RecipeId) : CommandBase;